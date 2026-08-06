using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Services;
using NotificationService.Application.Validators;
using NotificationService.Application.Dtos;
#if !NETSTANDARD2_0
using FluentValidation.AspNetCore;
#endif

namespace NotificationService.Application
{
    /// <summary>
    /// Extension method for registering Application layer dependencies
    /// This includes business logic services, validators, and interfaces
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register the main notification request handler
            services.AddScoped<INotificationRequestHandler, NotificationRequestHandler>();

#if !NETSTANDARD2_0
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
#endif

            // Register FluentValidation validators explicitly
            services.AddScoped<IValidator<NotificationRequestDto>, NotificationRequestValidator>();
            services.AddScoped<IValidator<EmailNotificationDto>, EmailNotificationValidator>();
            services.AddScoped<IValidator<SmsNotificationDto>, SmsNotificationValidator>();

            return services;
        }
    }
}