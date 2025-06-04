using NotificationService.Application.Dtos;

namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Interface for handling notification requests from the API layer
    /// This is the main business logic entry point
    /// </summary>
    public interface INotificationRequestHandler
    {
        /// <summary>
        /// Handles an incoming notification request by:
        /// 1. Validating the request
        /// 2. Enqueuing the appropriate Hangfire job
        /// 3. Returning a job ID for tracking
        /// </summary>
        /// <param name="request">The notification request containing type and data</param>
        /// <returns>Hangfire job ID for tracking the notification</returns>
        Task<string> HandleAsync(NotificationRequestDto request);
    }
}
