namespace NotificationService.Application.Dtos
{
    /// <summary>
    /// Main request DTO that represents an incoming notification request
    /// This is what the API controller receives from external systems
    /// Validation is handled by FluentValidation (NotificationRequestValidator)
    /// </summary>
    public class NotificationRequestDto
    {
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Email-specific data (only populated when Type = "email")
        /// </summary>
        public EmailNotificationDto? Email { get; set; }

        /// <summary>
        /// SMS-specific data (only populated when Type = "sms")
        /// </summary>
        public SmsNotificationDto? Sms { get; set; }


        // TODO: Future enhancements - uncomment when needed
        // /// <summary>
        // /// Optional priority level for job processing
        // /// Higher values = higher priority in Hangfire queues
        // /// </summary>
        // public int Priority { get; set; } = 0;

        // /// <summary>
        // /// Optional delay before processing (in minutes)
        // /// Useful for scheduled notifications
        // /// </summary>
        // public int DelayMinutes { get; set; } = 0;
    }
}
