using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NotificationService.API.Controllers
{
    /// <summary>
    /// Health check controller for liveness and readiness probes
    /// Used by Kubernetes for health monitoring
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
        {
            _healthCheckService = healthCheckService;
            _logger = logger;
        }

        /// <summary>
        /// Liveness probe endpoint
        /// Returns 200 if the application is running
        /// </summary>
        [HttpGet("live")]
        public IActionResult GetLiveness()
        {
            _logger.LogDebug("Liveness probe called");
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }

        /// <summary>
        /// Readiness probe endpoint
        /// Returns 200 if the application is ready to serve traffic
        /// Checks database connectivity and external service availability
        /// </summary>
        [HttpGet("ready")]
        public async Task<IActionResult> GetReadiness()
        {
            _logger.LogDebug("Readiness probe called");
            
            try
            {
                var healthReport = await _healthCheckService.CheckHealthAsync();
                
                var response = new
                {
                    status = healthReport.Status.ToString().ToLowerInvariant(),
                    timestamp = DateTime.UtcNow,
                    duration = healthReport.TotalDuration.TotalMilliseconds,
                    checks = healthReport.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString().ToLowerInvariant(),
                        description = entry.Value.Description,
                        duration = entry.Value.Duration.TotalMilliseconds
                    })
                };

                return healthReport.Status == HealthStatus.Healthy 
                    ? Ok(response) 
                    : StatusCode(503, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new { status = "unhealthy", timestamp = DateTime.UtcNow, error = ex.Message });
            }
        }

        /// <summary>
        /// Startup probe endpoint
        /// Returns 200 when the application has fully started
        /// </summary>
        [HttpGet("startup")]
        public async Task<IActionResult> GetStartup()
        {
            _logger.LogDebug("Startup probe called");
            
            try
            {
                var healthReport = await _healthCheckService.CheckHealthAsync();
                
                return healthReport.Status == HealthStatus.Healthy 
                    ? Ok(new { status = "healthy", timestamp = DateTime.UtcNow })
                    : StatusCode(503, new { status = "unhealthy", timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Startup health check failed");
                return StatusCode(503, new { status = "unhealthy", timestamp = DateTime.UtcNow, error = ex.Message });
            }
        }
    }
}
