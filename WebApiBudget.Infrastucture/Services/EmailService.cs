using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models;

namespace WebApiBudget.Infrastucture.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailVerificationAsync(string email, string otpCode)
        {
            var subject = "Email Verification - Budget App";
            var body = $@"
                <html>
                <body>
                    <h2>Email Verification</h2>
                    <p>Thank you for registering with Budget App!</p>
                    <p>Your verification code is: <strong>{otpCode}</strong></p>
                    <p>This code will expire in {_emailSettings.OtpExpirationMinutes} minutes.</p>
                    <p>If you didn't request this verification, please ignore this email.</p>
                </body>
                </html>";

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendPasswordResetAsync(string email, string otpCode)
        {
            var subject = "Password Reset - Budget App";
            var body = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>You have requested to reset your password for Budget App.</p>
                    <p>Your password reset code is: <strong>{otpCode}</strong></p>
                    <p>This code will expire in {_emailSettings.OtpExpirationMinutes} minutes.</p>
                    <p>If you didn't request this password reset, please ignore this email.</p>
                </body>
                </html>";

            return await SendEmailAsync(email, subject, body);
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Validate email settings
                if (string.IsNullOrEmpty(_emailSettings.SmtpServer))
                {
                    _logger.LogError("SMTP Server not configured");
                    return false;
                }

                if (string.IsNullOrEmpty(_emailSettings.SmtpUsername) || string.IsNullOrEmpty(_emailSettings.SmtpPassword))
                {
                    _logger.LogError("SMTP credentials not configured");
                    return false;
                }

                _logger.LogInformation("Attempting to send email to {Email} via {SmtpServer}:{SmtpPort}", to, _emailSettings.SmtpServer, _emailSettings.SmtpPort);

                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                client.EnableSsl = _emailSettings.EnableSsl;
                client.Timeout = 30000; // 30 seconds timeout

                using var message = new MailMessage();
                message.From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                await client.SendMailAsync(message);
                
                _logger.LogInformation("Email sent successfully to {Email}", to);
                return true;
            }
            catch (SmtpException smtpEx)
            {
                _logger.LogError(smtpEx, "SMTP error while sending email to {Email}: {Message}", to, smtpEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error while sending email to {Email}: {Message}", to, ex.Message);
                return false;
            }
        }
    }
}