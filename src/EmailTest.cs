using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Dtos;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Test
{
    public class EmailTest
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Starting email test...");

            // Create logger
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<EmailService>();

            // Create Mailtrap settings
            var mailtrapSettings = new MailtrapSettings
            {
                Host = "sandbox.smtp.mailtrap.io",
                Port = 2525,
                Username = "be3db024a8e53f",
                Password = "2832f3914fae9a",
                FromEmail = "noreply@paysky-notifications.com",
                FromName = "PaySky Notification Service",
                EnableSsl = false
            };

            var options = Options.Create(mailtrapSettings);

            // Create email service
            var emailService = new EmailService(options, logger);

            // Create test email
            var testEmail = new EmailNotificationDto
            {
                To = "test@example.com",
                Subject = "Test Email from Notification Service",
                Body = "This is a test email to verify the email service is working correctly.",
                IsHtml = false
            };

            try
            {
                Console.WriteLine("Sending test email...");
                await emailService.SendNotificationAsync(testEmail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
