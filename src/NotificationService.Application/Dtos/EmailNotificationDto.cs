namespace NotificationService.Application.Dtos
{
    /// <summary>
    /// DTO for email notification requests
    /// Contains all data needed to send an email via Mailtrap
    /// Validation is handled by FluentValidation (EmailNotificationValidator)
    /// </summary>
    public class EmailNotificationDto
    {
        public string To { get; set; } = string.Empty;


        public string? From { get; set; }

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public bool IsHtml { get; set; } = false;

        public List<string> Cc { get; set; } = new();

        public List<string> Bcc { get; set; } = new();
    }
}
