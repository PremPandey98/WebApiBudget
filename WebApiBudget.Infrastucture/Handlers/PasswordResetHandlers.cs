using Budget.Application.Authentication.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApiBudget.Application.Authentication.Commands;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.Infrastucture.Data;

namespace WebApiBudget.Infrastucture.Handlers
{
    public class SendPasswordResetHandler : IRequestHandler<SendPasswordResetCommand, bool>
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;
        private readonly ILogger<SendPasswordResetHandler> _logger;

        public SendPasswordResetHandler(
            IOtpService otpService,
            IEmailService emailService,
            AppDbContext context,
            ILogger<SendPasswordResetHandler> logger)
        {
            _otpService = otpService;
            _emailService = emailService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(SendPasswordResetCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing password reset request for {Email}", request.Email);

                // Check if user exists (only using basic user properties)
                var user = await _context.Users
                    .Where(u => u.Email.ToLower() == request.Email.ToLower())
                    .Select(u => new { u.UserId, u.Email }) // Only select needed properties
                    .FirstOrDefaultAsync(cancellationToken);
                
                if (user == null) 
                {
                    _logger.LogWarning("User with email {Email} not found for password reset", request.Email);
                    return false;
                }

                _logger.LogInformation("User found, generating OTP for password reset {Email}", request.Email);

                // Generate OTP (stored in cache)
                var otpCode = await _otpService.GenerateOtpAsync(request.Email, OtpType.PasswordReset);

                _logger.LogInformation("OTP generated: {OtpCode}, attempting to send password reset email to {Email}", otpCode, request.Email);

                // Send email
                var emailSent = await _emailService.SendPasswordResetAsync(request.Email, otpCode);
                
                if (emailSent)
                {
                    _logger.LogInformation("Password reset email sent successfully to {Email}", request.Email);
                }
                else
                {
                    _logger.LogError("Failed to send password reset email to {Email}", request.Email);
                }

                return emailSent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing password reset for {Email}: {Message}", request.Email, ex.Message);
                return false;
            }
        }
    }

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IOtpService _otpService;
        private readonly AppDbContext _context;
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(IOtpService otpService, AppDbContext context, ILogger<ResetPasswordHandler> logger)
        {
            _otpService = otpService;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing password reset for {Email} with OTP {OtpCode}", request.Email, request.OtpCode);

                // Validate OTP (from cache)
                var isValidOtp = await _otpService.ValidateOtpAsync(request.Email, request.OtpCode, OtpType.PasswordReset);
                if (!isValidOtp) 
                {
                    _logger.LogWarning("Invalid OTP provided for password reset {Email}", request.Email);
                    return false;
                }

                // Update user password (only accessing existing columns)
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);
                
                if (user == null) 
                {
                    _logger.LogWarning("User with email {Email} not found during password reset", request.Email);
                    return false;
                }

                // In production, hash the password properly
                user.Password = request.NewPassword;
                user.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("Password reset successful for {Email}", request.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for {Email}: {Message}", request.Email, ex.Message);
                return false;
            }
        }
    }
}