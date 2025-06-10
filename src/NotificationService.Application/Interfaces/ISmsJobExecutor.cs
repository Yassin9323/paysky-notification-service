using NotificationService.Application.Dtos;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Interface for SMS job execution in background processing
    /// This is called by Hangfire when processing SMS jobs from the "sms" queue
    /// </summary>
    public interface ISmsJobExecutor
    {
        /// <summary>
        /// Executes SMS notification job asynchronously
        /// This method will be called by Hangfire background server
        /// </summary>
        /// <param name="smsDto">SMS notification details</param>
        /// <returns>Task representing the async operation</returns>
        Task ExecuteAsync(SmsNotificationDto smsDto);
    }
}