using Hangfire.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Hangfire;
using Microsoft.Extensions.Configuration;
using NotificationService.Infrastructure.Logging;
using NotificationService.Infrastructure.Configuration;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.BackgroundJobs;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Hangfire with PostgreSQL storage
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(configuration.GetConnectionString("HangfireConnection")))
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                );

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "email", "sms", "any" };
            });

            // Configure logging
            var loggerSettings = configuration
                .GetSection("LoggerSettings")
                .Get<LoggerSettings>();

            if (loggerSettings != null)
            {
                services.RegisterLoggerDependencies(loggerSettings);
            }
            
            // Configure external service settings
            services.Configure<VongageSettings>(configuration.GetSection("VongageSettings"));
            services.Configure<MailtrapSettings>(configuration.GetSection("MailtrapSettings"));
            
            // Register notification services (implementing INotificationService<T>)
            services.AddTransient<INotificationService<SmsNotificationDto>, SmsService>();
            services.AddTransient<INotificationService<EmailNotificationDto>, EmailService>();
            
            // Register job executors for Hangfire
            services.AddTransient<ISmsJobExecutor, SmsJobExecutor>();
            services.AddTransient<IEmailJobExecutor, EmailJobExecutor>();
            
            return services;
        }
    }
}