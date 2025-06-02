// using Microsoft.Extensions.Logging.Abstractions;
// using Microsoft.Extensions.Options;
// using NotificationService.Application.Dtos;
// using NotificationService.Infrastructure.configuration;
// using NotificationService.Infrastructure.Services;
// using Hangfire;
// using NotificationService.Infrastructure.BackgroundJobs;
// using System.Threading.Tasks;
// using Moq;
// using Xunit;
// using Hangfire.Common;
// using Hangfire.States;


// namespace NotificationService.Infrastructure.Tests
// {
// public class SmsServiceTests
// {
//     [Fact(Skip = "Run this manually to avoid spamming real numbers.")]
//     public async Task SendNotificationAsync_SendsSmsSuccessfully()
//     {
//         // Arrange
//         var settings = new VongageSettings
//         {
//             ApiKey = "204ad9c3",
//             ApiSecret = "Ib3iSnULUokfRphH"
//         };

//         var options = Options.Create(settings);
//         var logger = NullLogger<SmsService>.Instance; // Use NullLogger or mock if needed

//         var service = new SmsService(logger, options);

//         var smsDto = new SmsNotificationDto
//         {
//             To = "+201270079243",
//             From = "+201270079243",
//             Message = "Test SMS from integration test"
//         };

//         // Act
//         await service.SendNotificationAsync(smsDto);

//         // Assert
//         // Check logs manually or monitor Vonage dashboard for delivery.
//     }
// }
//     public class NotificationJobSchedulerTests
// {
//     [Fact]
//     public void ScheduleNotificationJob_EnqueuesJobWithCorrectQueueAndJob()
//     {
//         // Arrange
//         var mockBackgroundJobClient = new Mock<IBackgroundJobClient>();

//         var scheduler = new NotificationJobScheduler(mockBackgroundJobClient.Object);

//         var notification = new NotificationDto
//         {
//             Type = "Sms",
//             To = "123",
//             From = "321",
//             Content = "Test SMS content"
//         };

//         // Act
//         scheduler.ScheduleNotificationJob(notification);

//         // Assert
//         mockBackgroundJobClient.Verify(client => client.Create(
//             It.Is<Job>(job =>
//                 job.Type == typeof(SmsJobExecutor) &&
//                 job.Method.Name == "ExecuteAsync" &&
//                 job.Args.Count == 1 &&
//                 job.Args[0] == notification),
//             It.Is<EnqueuedState>(state => state.Queue == "sms")
//         ), Times.Once);
//     }
// }
// }
// using System;
// using System.Threading.Tasks;
// using Hangfire;
// using Hangfire.PostgreSql;
// using Microsoft.Extensions.DependencyInjection;
// using NotificationService.Application.Dtos;
// using NotificationService.Infrastructure.BackgroundJobs;
// using Xunit;

// public class RealHangfireIntegrationTest : IDisposable
// {
//     private readonly IServiceProvider _serviceProvider;
//     private readonly BackgroundJobServer _hangfireServer;

//     public RealHangfireIntegrationTest()
//     {
//         var services = new ServiceCollection();

//         // Add Hangfire and configure PostgreSQL storage
//         services.AddHangfire(configuration =>
//             configuration.UsePostgreSqlStorage("Host=localhost;Port=5432;Database=Hangfire;Username=postgres;Password=ahehemsa01;"));

//         // Register jobs and scheduler
//         services.AddTransient<SmsJobExecutor>();
//         services.AddTransient<NotificationJobScheduler>();
//         services.AddTransient<IBackgroundJobClient, BackgroundJobClient>();

//         _serviceProvider = services.BuildServiceProvider();

//         // Set JobStorage.Current manually for static API compatibility
//         GlobalConfiguration.Configuration.UsePostgreSqlStorage("Host=localhost;Port=5432;Database=Hangfire;Username=postgres;Password=ahehemsa01;");
//         JobStorage.Current = new PostgreSqlStorage("Host=localhost;Port=5432;Database=Hangfire;Username=postgres;Password=ahehemsa01;");

//         // Start the Hangfire server manually
//         _hangfireServer = new BackgroundJobServer(new BackgroundJobServerOptions
//         {
//             Queues = new[] { "sms" }
//         });
//     }

//     [Fact]
//     public async Task ScheduleNotificationJob_EnqueuesJobSuccessfully()
//     {
//         using var scope = _serviceProvider.CreateScope();

//         var scheduler = scope.ServiceProvider.GetRequiredService<NotificationJobScheduler>();

//         var notification = new NotificationDto
//         {
//             Type = "Sms",
//             To = "+201270079243",
//             From = "+201270079243",
//             Content = "Test SMS"
//         };

//         scheduler.ScheduleNotificationJob(notification);

//         await Task.Delay(5000); // Wait for job to be processed
//     }

//     public void Dispose()
//     {
//         _hangfireServer?.Dispose();
//     }
// }
