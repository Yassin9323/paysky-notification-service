using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;
using NotificationService.Infrastructure.Configuration;

namespace NotificationService.Infrastructure.Services
{
    /// <summary>
    /// Email service implementation using Mailtrap SMTP
    /// Implements INotificationService<EmailNotificationDto> for consistency
    /// </summary>
    public class EmailService : INotificationService<EmailNotificationDto>
    {
        private readonly MailtrapSettings _mailtrapSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<MailtrapSettings> mailtrapSettings,
            ILogger<EmailService> logger)
        {
            _mailtrapSettings = mailtrapSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends email notification using Mailtrap SMTP
        /// </summary>
        /// <param name="notification">Email notification details</param>
        public async Task SendNotificationAsync(EmailNotificationDto notification)
        {
            try
            {
                _logger.LogInformation("Sending email to {ToEmail} with subject: {Subject}", 
                    notification.To, notification.Subject);

                using var smtpClient = CreateSmtpClient();
                using var mailMessage = CreateMailMessage(notification);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation("Successfully sent email to {ToEmail}", notification.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", notification.To);
                throw;
            }
        }

        /// <summary>
        /// Creates and configures SMTP client with Mailtrap settings
        /// </summary>
        private SmtpClient CreateSmtpClient()
        {
            var client = new SmtpClient(_mailtrapSettings.Host, _mailtrapSettings.Port)
            {
                Credentials = new NetworkCredential(_mailtrapSettings.Username, _mailtrapSettings.Password),
                EnableSsl = _mailtrapSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Timeout = 30000 // 30 seconds timeout
            };

            _logger.LogInformation("SMTP client configured for {Host}:{Port}, SSL: {EnableSsl}, Username: {Username}", 
                _mailtrapSettings.Host, _mailtrapSettings.Port, _mailtrapSettings.EnableSsl, _mailtrapSettings.Username);

            return client;
        }

        /// <summary>
        /// Creates mail message from email notification DTO
        /// </summary>
        private MailMessage CreateMailMessage(EmailNotificationDto notification)
        {
            var mailMessage = new MailMessage();

            // Set From address - use provided or default
            var fromAddress = string.IsNullOrEmpty(notification.From) 
                ? _mailtrapSettings.FromEmail 
                : notification.From;
            
            mailMessage.From = new MailAddress(fromAddress, _mailtrapSettings.FromName);

            // Set To address
            mailMessage.To.Add(notification.To);

            // Add CC recipients if provided
            if (notification.Cc?.Any() == true)
            {
                foreach (var cc in notification.Cc)
                {
                    mailMessage.CC.Add(cc);
                }
            }

            // Add BCC recipients if provided
            if (notification.Bcc?.Any() == true)
            {
                foreach (var bcc in notification.Bcc)
                {
                    mailMessage.Bcc.Add(bcc);
                }
            }

            // Set subject and body
            mailMessage.Subject = notification.Subject;
            mailMessage.Body = notification.Body;
            mailMessage.IsBodyHtml = notification.IsHtml;

            // Set encoding
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

            _logger.LogDebug("Mail message created for {ToEmail} with {CcCount} CC and {BccCount} BCC recipients",
                notification.To, notification.Cc?.Count ?? 0, notification.Bcc?.Count ?? 0);

            return mailMessage;
        }
    }
}
