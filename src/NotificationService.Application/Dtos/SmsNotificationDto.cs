namespace NotificationService.Application.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for SMS notifications.
    /// Validation is handled by FluentValidation (SmsNotificationValidator)
    /// </summary>
    public class SmsNotificationDto
    {
        /// <summary>
        /// Recipient's phone number in international format
        /// </summary>
        public string To { get; set; } = string.Empty;
        
        /// <summary>
        /// Sender's phone number (optional, will use default if not provided)
        /// </summary>
        public string From { get; set; } = string.Empty;
        
        /// <summary>
        /// SMS message content (max 160 characters for single SMS)
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}