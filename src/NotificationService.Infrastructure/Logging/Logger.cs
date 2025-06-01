using NotificationService.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Logging;
public static class LoggingExtensions
{
    public static void RegisterLoggerDependencies(this IServiceCollection services, LoggerSettings loggerSettings)
    {
        ElasticsearchSinkOptions elasticsearchSinkOptions = new ElasticsearchSinkOptions(new Uri(loggerSettings.ElasticSearchUrl))
        {
            AutoRegisterTemplate = true,
            IndexFormat = loggerSettings.IndexFormat,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7
        };

        if (!string.IsNullOrWhiteSpace(loggerSettings.ElasticUserName))
        {
            elasticsearchSinkOptions.ModifyConnectionSettings = x =>
                x.BasicAuthentication(loggerSettings.ElasticUserName, loggerSettings.ElasticPassword)
                .ServerCertificateValidationCallback((o, cert, chain, errors) => true);
        }

        var logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(LogEventLevel.Verbose)
            .WriteTo.Elasticsearch(elasticsearchSinkOptions)
            .WriteTo.File(loggerSettings.FilePath, LogEventLevel.Verbose)
            .CreateLogger();

        services.AddLogging(delegate (ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(logger);
        });

    }
}

