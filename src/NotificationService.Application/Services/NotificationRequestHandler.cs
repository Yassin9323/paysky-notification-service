using Hangfire;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;
using Microsoft.Extensions.Logging;

namespace NotificationService.Application.Services
{
    /// <summary>
    /// Implementation of INotificationRequestHandler
    /// This class handles the business logic for processing notification requests
    /// and enqueuing them as Hangfire background jobs
    /// </summary>
    public class NotificationRequestHandler : INotificationRequestHandler
    {
        private readonly ILogger<NotificationRequestHandler> _logger;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public NotificationRequestHandler(
            ILogger<NotificationRequestHandler> logger,
            IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _backgroundJobClient = backgroundJobClient;
        }

        /// <summary>
        /// Handles notification requests by routing them to appropriate Hangfire jobs
        /// </summary>
        /// <param name="request">The validated notification request</param>
        /// <returns>Hangfire job ID for tracking</returns>
        public async Task<string> HandleAsync(NotificationRequestDto request)
        {
            _logger.LogInformation("Processing notification request of type: {Type}", request.Type);

            try
            {
                string jobId = request.Type.ToLower() switch
                {
                    "sms" => await HandleSmsNotification(request),
                    "email" => await HandleEmailNotification(request),
                    _ => throw new ArgumentException($"Unsupported notification type: {request.Type}")
                };

                _logger.LogInformation("Successfully enqueued {Type} notification with job ID: {JobId}", 
                    request.Type, jobId);

                return jobId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to enqueue notification of type: {Type}", request.Type);
                throw;
            }
        }

        /// <summary>
        /// Handles SMS notification requests
        /// Enqueues job to "sms" queue for background processing
        /// </summary>
        private Task<string> HandleSmsNotification(NotificationRequestDto request)
        {
            if (request.Sms == null)
            {
                throw new ArgumentException("SMS details are required for SMS notifications");
            }

            _logger.LogDebug("Enqueuing SMS notification to {PhoneNumber}", request.Sms.To);

            // Enqueue to "sms" queue as defined in your SDD
            var jobId = _backgroundJobClient.Enqueue<ISmsJobExecutor>(
                "sms", 
                executor => executor.ExecuteAsync(request.Sms));

            return Task.FromResult(jobId);
        }

        /// <summary>
        /// Handles Email notification requests  
        /// Enqueues job to "email" queue for background processing
        /// </summary>
        private Task<string> HandleEmailNotification(NotificationRequestDto request)
        {
            if (request.Email == null)
            {
                throw new ArgumentException("Email details are required for email notifications");
            }

            _logger.LogDebug("Enqueuing email notification to {EmailAddress}", request.Email.To);

            // Enqueue to "email" queue as defined in your SDD
            var jobId = _backgroundJobClient.Enqueue<IEmailJobExecutor>(
                "email", 
                executor => executor.ExecuteAsync(request.Email));

            return Task.FromResult(jobId);
        }
    }
}
