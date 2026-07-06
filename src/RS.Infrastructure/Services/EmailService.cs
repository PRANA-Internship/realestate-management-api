using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RS.Application.Common.Interfaces;
using RS.Infrastructure.Email;

namespace RS.Infrastructure.Services
{
    public class EmailService(ILogger<EmailService> logger, IConfiguration configuration) : IEmailService
    {

        private async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct)
        {
            var host = configuration["SmtpSettings:Host"];
            var port = int.Parse(configuration["SmtpSettings:Port"] ?? "587");
            var enableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"] ?? "true");
            var username = configuration["SmtpSettings:Username"];
            var password = configuration["SmtpSettings:Password"];
            var fromEmail = configuration["SmtpSettings:FromEmail"];
            var fromName = configuration["SmtpSettings:FromName"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username))
            {
                logger.LogWarning("SMTP settings are not fully configured. Email was not sent.");
                return;
            }

            try
            {
                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage, ct);
                logger.LogInformation($"Email sent successfully to {toEmail} with subject '{subject}'");
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, $"Failed to send email to {toEmail}");
            }
        }

        public async Task SendPasswordResetAsync(string toEmail, string fullName, string token, CancellationToken ct = default)
        {
            var subject = "Password Reset Request";
            var body = $"<h1>Hello {fullName},</h1><p>You requested a password reset. Use this token: <strong>{token}</strong></p>";
            await SendEmailAsync(toEmail, subject, body, ct);
        }

        public async Task SendPasswordSetupAsync(
     string toEmail,
     string fullName,
     string activationUrl,
     CancellationToken ct = default)
        {
            var subject = "Activate your Manager Account";

            var body = $@"
        <h2>Welcome {fullName},</h2>

        <p>Your manager account has been created successfully.</p>

        <p>Please click the button below to set your password.</p>

        <p>
            <a href='{activationUrl}'
               style='padding:12px 20px;
                      background:#2563EB;
                      color:white;
                      text-decoration:none;
                      border-radius:6px'>
                Set Password
            </a>
        </p>

        <p>This link expires in 24 hours.</p>

        <p>If the button doesn't work:</p>

        <p>{activationUrl}</p>";
            logger.LogInformation("Password reset initiated for user: {ToEmail}. Link: {ActivationUrl}", toEmail, activationUrl);
            await SendEmailAsync(
                toEmail,
                subject,
                body,
                ct);
        }

        public async Task SendWelcomeEmailAsync(
      string toEmail,
      string fullName,
      CancellationToken ct = default)
        {
            var loginUrl = $"{configuration["AppSettings:FrontendUrl"]}/login";

            var body = EmailTemplate.WelcomeEmail(
                fullName,
                loginUrl);

            await SendEmailAsync(
                toEmail,
                "Welcome to RealEstate System",
                body,
                ct);
        }
    }
}
