namespace NotificationService.Application.Interfaces
{
    /// <summary>
    /// Interface for notification service that sends notifications of type TNotification.
    /// /// </summary>
    /// /// <typeparam name="TNotification">The type of notification to be sent.</typeparam>
    /// /// <remarks>
    /// This interface defines a contract for sending notifications asynchronously.
    /// /// Implementations of this interface should handle the logic for sending notifications,
    /// /// such as email, SMS, or push notifications.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// /// <code>
    /// /// public class EmailNotificationService : INotificationService<EmailNotification>
    /// /// {
    /// ///     public async Task SendNotificationAsync(EmailNotificationDto notification)
    /// ///     {
    /// ///         // Logic to send email notification
    /// ///     }
    /// /// }
    /// /// </code>
    public interface INotificationService<TNotification>
    {
        Task SendNotificationAsync(TNotification notification);
    }
}