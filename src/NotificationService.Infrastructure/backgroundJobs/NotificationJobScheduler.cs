using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using NotificationService.Application.Dtos;

namespace NotificationService.Infrastructure.BackgroundJobs
{
    /// <summary>
    /// Schedules background jobs for sending notifications using Hangfire.
    /// This class is responsible for determining the appropriate job executor based on the notification type
    /// </summary>
    /// <remarks>
    /// The `NotificationJobScheduler` class uses Hangfire to schedule jobs for sending notifications.
    /// It determines the job executor type based on the notification type and schedules the job accordingly.
    /// </remarks>
    /// <example>
    /// /// Example usage:
    /// /// <code>
    /// /// var scheduler = new NotificationJobScheduler(backgroundJobClient);
    /// /// var notification = new NotificationDto { Type = "Sms", To = "1234567890", From = "0987654321", Content = "Hello World" };
    /// /// /// scheduler.ScheduleNotificationJob(notification);
    /// /// </code>
    /// /// </example>
    public class NotificationJobScheduler
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public NotificationJobScheduler(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public void ScheduleNotificationJob(NotificationDto notification)
        {
            string queueName = notification.Type.ToString().ToLower() switch
            {
                "sms" => "sms",
                // "email" => "email",
                _ => throw new ArgumentException("Unsupported notification type")
            };

            Type jobExecutorType = queueName switch
            {
                "sms" => typeof(SmsJobExecutor),
                // "email" => typeof(EmailJobExecutor),
                _ => throw new InvalidOperationException()
            };

            var methodInfo = jobExecutorType.GetMethod("ExecuteAsync",
                new[] { typeof(NotificationDto) });

            if (methodInfo == null)
                throw new InvalidOperationException($"ExecuteAsync(NotificationDto) not found in {jobExecutorType.Name}");

            var job = new Job(jobExecutorType, methodInfo, new object[] { notification });

            _backgroundJobClient.Create(job, new EnqueuedState(queueName));
        }
    }
}
