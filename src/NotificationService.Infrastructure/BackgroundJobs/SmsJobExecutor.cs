using Microsoft.Extensions.Logging;
using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;

namespace NotificationService.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Hangfire job executor for SMS notifications
    /// This class is called by Hangfire when processing jobs from the "sms" queue
    /// </summary>
    public class SmsJobExecutor : ISmsJobExecutor
    {
        private readonly INotificationService<SmsNotificationDto> _smsService;
        private readonly ILogger<SmsJobExecutor> _logger;

        public SmsJobExecutor(
            INotificationService<SmsNotificationDto> smsService,
            ILogger<SmsJobExecutor> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        /// <summary>
        /// Executes SMS notification job
        /// Called by Hangfire background server from the "sms" queue
        /// </summary>
        /// <param name="smsDto">SMS notification details</param>
        public async Task ExecuteAsync(SmsNotificationDto smsDto)
        {
            _logger.LogInformation("Starting SMS job execution for {PhoneNumber}", smsDto.To);

            try
            {
                // Delegate actual SMS sending to the SMS service
                await _smsService.SendNotificationAsync(smsDto);

                _logger.LogInformation("SMS job completed successfully for {PhoneNumber}", smsDto.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMS job failed for {PhoneNumber}", smsDto.To);
                // Re-throw to let Hangfire handle retries as per your SDD
                throw;
            }
        }
    }
}