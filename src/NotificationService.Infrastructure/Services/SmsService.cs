using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.configuration;
using Vonage;
using NotificationService.Infrastructure.Logging;

namespace NotificationService.Infrastructure.Services
{
    /// <summary>
    /// Represents a service for sending SMS notifications.
    /// </summary>
    public class SmsService : INotificationService<SmsNotificationDto>
    {
        private readonly ILogger<SmsService> _logger;
        private readonly VongageSettings _vongageSettings;

        public SmsService(ILogger<SmsService> logger, IOptions<VongageSettings> vongageSettings)
        {
            _vongageSettings = vongageSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends an SMS notification using Vongo.
        /// </summary>
        /// <param name="smsNotification">The SMS notification to send.</param>
        public async Task SendNotificationAsync(SmsNotificationDto smsNotification)
        {
            try
            {
                var credentials = Vonage.Request.Credentials.FromApiKeyAndSecret(
                    _vongageSettings.ApiKey,
                    _vongageSettings.ApiSecret
                );
                var client = new VonageClient(credentials);

                var response = await client.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest
                {
                    To = smsNotification.To,
                    From = smsNotification.From,
                    Text = smsNotification.Message
                });

                if (response.Messages.Any(m => m.Status != "0"))
                {
                    _logger.LogError($"Failed to send SMS: {response.Messages.FirstOrDefault()?.ErrorText}");
                }
                _logger.LogInformation($"SMS sent successfully to {smsNotification.To} from {smsNotification.From}. Message: {smsNotification.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending SMS notification.");
            }
        }
    }
}