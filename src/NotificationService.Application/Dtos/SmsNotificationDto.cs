namespace NotificationService.Application.Dtos
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for SMS notifications.
    /// </summary>
    public class SmsNotificationDto
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
    }
}