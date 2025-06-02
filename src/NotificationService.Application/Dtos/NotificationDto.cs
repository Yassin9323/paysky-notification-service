namespace NotificationService.Application.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for notifications.
    /// </summary>
    public class NotificationDto
    {
        /// <summary>
        /// Gets or sets the type of the notification (e.g., SMS, Email).
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the content of the notification.
        /// </summary>
        public string Content { get; set; }
        public string To { get; set; }
        public string From { get; set; }
    }
}