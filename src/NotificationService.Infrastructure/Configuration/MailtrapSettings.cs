namespace NotificationService.Infrastructure.Configuration
{
    /// <summary>
    /// Configuration settings for Mailtrap SMTP service
    /// These settings are loaded from appsettings.json
    /// </summary>
    public class MailtrapSettings
    {
        /// <summary>
        /// Mailtrap SMTP host (sandbox.smtp.mailtrap.io)
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// SMTP port (25, 465, 587 or 2525) - using 2525 for development
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Mailtrap username for authentication
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Mailtrap password for authentication
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Default "From" email address for outgoing emails
        /// </summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Default "From" display name for outgoing emails
        /// </summary>
        public string FromName { get; set; } = string.Empty;

        /// <summary>
        /// Whether to enable SSL/TLS (true for production, optional for Mailtrap)
        /// </summary>
        public bool EnableSsl { get; set; } = true;
    }
}
