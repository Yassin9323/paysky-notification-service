using Microsoft.Extensions.Logging;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;

namespace NotificationService.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Hangfire job executor for email notifications
    /// This class is called by Hangfire when processing jobs from the "email" queue
    /// </summary>
    public class EmailJobExecutor : IEmailJobExecutor
    {
        private readonly INotificationService<EmailNotificationDto> _emailService;
        private readonly ILogger<EmailJobExecutor> _logger;

        public EmailJobExecutor(
            INotificationService<EmailNotificationDto> emailService,
            ILogger<EmailJobExecutor> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Executes email notification job
        /// Called by Hangfire background server from the "email" queue
        /// </summary>
        /// <param name="emailDto">Email notification details</param>
        public async Task ExecuteAsync(EmailNotificationDto emailDto)
        {
            _logger.LogInformation("Starting email job execution for {ToEmail}", emailDto.To);

            try
            {
                emailDto.From = "ahmed.eh01@gmail.com";
                // Delegate actual email sending to the email service
                await _emailService.SendNotificationAsync(emailDto);

                _logger.LogInformation("Email job completed successfully for {ToEmail}", emailDto.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email job failed for {ToEmail}", emailDto.To);
                
                // Re-throw to let Hangfire handle retries as per your SDD
                throw;
            }
        }
    }
}
