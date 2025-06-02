using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;

namespace NotificationService.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Represents a job executor for sending SMS notifications.
    /// </summary>
    public class SmsJobExecutor
    {
        private readonly INotificationService<SmsNotificationDto> _smsService;

        public SmsJobExecutor(INotificationService<SmsNotificationDto> smsService)
        {
            _smsService = smsService;
        }

        /// <summary>
        /// Executes the job to send an SMS notification.
        /// </summary>
        /// <param name="notification">The SMS notification to send.</param>
        public async Task ExecuteAsync(NotificationDto notification)
        {
            var smsNotification = new SmsNotificationDto
            {
                To = notification.To,
                From = notification.From,
                Message = notification.Content
            };
            await _smsService.SendNotificationAsync(smsNotification);
        }
    }
}