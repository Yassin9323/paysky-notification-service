using Hangfire.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using Hangfire;
using Microsoft.Extensions.Configuration;
using NotificationService.Infrastructure.Logging;

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

            services.AddHangfireServer();

            var loggerSettings = configuration
            .GetSection("LoggerSettings")
            .Get<LoggerSettings>();

            services.RegisterLoggerDependencies(loggerSettings);

            return services;
        }
    }
}