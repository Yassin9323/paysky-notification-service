namespace NotificationService.Infrastructure.configuration
{
    /// <summary>
    /// Represents the settings for Vongo SMS service.
    /// </summary>
    public class VongageSettings
    {
        /// <summary>
        /// Gets or sets the API key for Vongo SMS service.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the API secret for Vongo SMS service.
        /// </summary>
        public string ApiSecret { get; set; }

    }
}