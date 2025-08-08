using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiBudget.Application.Authentication.Commands;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Handlers
{
    public class SendEmailVerificationHandler : IRequestHandler<SendEmailVerificationCommand, bool>
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly ILogger<SendEmailVerificationHandler> _logger;

        public SendEmailVerificationHandler(
            IOtpService otpService,
            IEmailService emailService,
            AppDbContext context,
            ILogger<SendEmailVerificationHandler> logger)
        {
            _otpService = otpService;
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing email verification request for {Email}", request.Email);

                // Check if user exists (only using basic user properties)
                var user = await _context.Users
                    .Where(u => u.Email.ToLower() == request.Email.ToLower())
                    .Select(u => new { u.UserId, u.Email }) // Only select needed properties
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (user == null) 
                {
                    _logger.LogWarning("User with email {Email} not found. Allowing email verification for registration purposes.", request.Email);
                    // Allow email verification for non-existing users (useful for registration flow)
                }
                else
                {
                    _logger.LogInformation("User found, generating OTP for {Email}", request.Email);
                }

                // Generate OTP (stored in cache) - regardless of user existence
                var otpCode = await _otpService.GenerateOtpAsync(request.Email, OtpType.EmailVerification);

                _logger.LogInformation("OTP generated: {OtpCode}, attempting to send email to {Email}", otpCode, request.Email);

                // Send email
                var emailSent = await _emailService.SendEmailVerificationAsync(request.Email, otpCode);
                
                if (emailSent)
                {
                    _logger.LogInformation("Email verification sent successfully to {Email}", request.Email);
                }
                else
                {
                    _logger.LogError("Failed to send email verification to {Email}", request.Email);
                }

                return emailSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email verification for {Email}: {Message}", request.Email, ex.Message);
                return false;
            }
        }
    }

    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly IOtpService _otpService;
        private readonly AppDbContext _context;
        private readonly ILogger<VerifyEmailHandler> _logger;

        public VerifyEmailHandler(IOtpService otpService, AppDbContext context, ILogger<VerifyEmailHandler> logger)
        {
            _otpService = otpService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing email verification for {Email} with OTP {OtpCode}", request.Email, request.OtpCode);

                // Validate OTP (from cache)
                var isValidOtp = await _otpService.ValidateOtpAsync(request.Email, request.OtpCode, OtpType.EmailVerification);
                if (!isValidOtp) 
                {
                    _logger.LogWarning("Invalid OTP provided for {Email}", request.Email);
                    return false;
                }

                // Just verify that user exists (we'll track verification in cache or separate system)
                var userExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);
                
                if (!userExists) 
                {
                    _logger.LogWarning("User with email {Email} not found during verification", request.Email);
                    return false;
                }

                _logger.LogInformation("Email verification successful for {Email}", request.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying email for {Email}: {Message}", request.Email, ex.Message);
                return false;
            }
        }
    }
}