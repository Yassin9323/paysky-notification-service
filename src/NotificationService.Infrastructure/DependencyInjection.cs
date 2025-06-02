using Hangfire.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Hangfire;
using Microsoft.Extensions.Configuration;
using NotificationService.Infrastructure.Logging;
using NotificationService.Infrastructure.configuration;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Dtos;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Hangfire with PostgreSQL storage
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection"))
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                );

            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "email", "sms", "any" };
            });

            var loggerSettings = configuration
            .GetSection("LoggerSettings")
            .Get<LoggerSettings>();

            services.RegisterLoggerDependencies(loggerSettings);
            services.Configure<VongageSettings>(configuration.GetSection("VongageSettings"));
            services.AddTransient<INotificationService<SmsNotificationDto>, SmsService>();
            return services;
        }
    }
}