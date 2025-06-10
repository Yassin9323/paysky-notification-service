using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Services;
using NotificationService.Application.Validators;
using NotificationService.Application.Dtos;
using FluentValidation.AspNetCore;

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

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddFluentValidationAutoValidation();

            // Register FluentValidation validators explicitly
            services.AddScoped<IValidator<NotificationRequestDto>, NotificationRequestValidator>();
            services.AddScoped<IValidator<EmailNotificationDto>, EmailNotificationValidator>();
            services.AddScoped<IValidator<SmsNotificationDto>, SmsNotificationValidator>();

            return services;
        }
    }
}