using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;
using FluentValidation;

namespace NotificationService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRequestHandler _notificationHandler;
        private readonly ILogger<NotificationsController> _logger;
        private readonly IValidator<NotificationRequestDto> _validator;

        public NotificationsController(
            INotificationRequestHandler notificationHandler,
            ILogger<NotificationsController> logger,
            IValidator<NotificationRequestDto> validator)
        {
            _notificationHandler = notificationHandler;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Accepts notification requests and enqueues them for background processing
        /// </summary>
        /// <param name="request">The notification request containing type and details</param>
        /// <returns>HTTP 202 Accepted with job ID</returns>
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
        {
            try
            {
                _logger.LogInformation("Received notification request of type: {NotificationType}", request.Type);

                // Validate using FluentValidation
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Invalid notification request received");
                    return BadRequest(validationResult.Errors.Select(e => new { 
                        Field = e.PropertyName, 
                        Error = e.ErrorMessage 
                    }));
                }

                // Enqueue the job and get job ID
                var jobId = await _notificationHandler.HandleAsync(request);

                _logger.LogInformation("Notification job enqueued with ID: {JobId}", jobId);

                // Return 202 Accepted immediately (fire-and-forget)
                return Accepted(new { JobId = jobId, Message = "Notification queued for processing" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notification request");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Health check endpoint for monitoring
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
        }
    }
}
