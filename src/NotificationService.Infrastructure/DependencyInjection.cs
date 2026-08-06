using Hangfire;
using Hangfire.AspNetCore;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.BackgroundJobs;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Infrastructure.Logging;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangfireConnectionString =
                configuration["NotificationService:Hangfire:ConnectionString"];

            var hangfireSchema =
                configuration["NotificationService:Hangfire:SchemaName"] ?? "Hangfire";

            if (string.IsNullOrWhiteSpace(hangfireConnectionString))
                throw new InvalidOperationException("Hangfire connection string is missing.");

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(hangfireConnectionString, new SqlServerStorageOptions
                      {
                          SchemaName = hangfireSchema,
                          SqlClientFactory = SqlClientFactory.Instance
                      });
            });

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "email", "sms", "any" };
            });

            var loggerSettings = configuration
                .GetSection("NotificationService:Logger")
                .Get<LoggerSettings>();

            if (loggerSettings != null)
            {
                services.RegisterLoggerDependencies(loggerSettings);
            }

            services.Configure<VongageSettings>(
                configuration.GetSection("NotificationService:Vongage"));

            services.Configure<MailtrapSettings>(
                configuration.GetSection("NotificationService:Mailtrap"));

            services.AddTransient<INotificationService<SmsNotificationDto>, SmsService>();
            services.AddTransient<INotificationService<EmailNotificationDto>, EmailService>();

            services.AddTransient<ISmsJobExecutor, SmsJobExecutor>();
            services.AddTransient<IEmailJobExecutor, EmailJobExecutor>();

            return services;
        }
    }
}