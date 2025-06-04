using NotificationService.Application.Dtos;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Interface for Email job execution in background processing
    /// This is called by Hangfire when processing Email jobs from the "email" queue
    /// </summary>
    public interface IEmailJobExecutor
    {
        /// <summary>
        /// Executes Email notification job asynchronously
        /// This method will be called by Hangfire background server
        /// </summary>
        /// <param name="emailDto">Email notification details</param>
        /// <returns>Task representing the async operation</returns>
        Task ExecuteAsync(EmailNotificationDto emailDto);
    }
}
