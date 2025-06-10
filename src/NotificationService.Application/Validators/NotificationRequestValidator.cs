using FluentValidation;
using NotificationService.Application.Dtos;

namespace NotificationService.Application.Validators
{
    /// <summary>
    /// FluentValidation validator for NotificationRequestDto
    /// This replaces the data annotations with more flexible validation rules
    /// </summary>
    public class NotificationRequestValidator : AbstractValidator<NotificationRequestDto>
    {
        public NotificationRequestValidator()
        {
            // Type validation - must be "email" or "sms"
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Notification type is required")
                .Must(type => type == "email" || type == "sms" || type == "any")
                .WithMessage("Type must be either 'email' or 'sms' or 'any'");


            // Conditional validation: if type is "email", Email object must be provided
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email details are required when type is 'email'")
                .When(x => x.Type == "email");

            // Conditional validation: if type is "sms", Sms object must be provided  
            RuleFor(x => x.Sms)
                .NotNull()
                .WithMessage("SMS details are required when type is 'sms'")
                .When(x => x.Type == "sms");

            RuleFor(x => x)
                .Must(x => x.Type != "any" || (x.Email != null && x.Sms != null))
                .WithMessage("Both Email and SMS details must be provided when type is 'any'");

            // Validate Email object when provided
            RuleFor(x => x.Email)
                .SetValidator(new EmailNotificationValidator()!)
                .When(x => x.Email != null);

            // Validate SMS object when provided
            RuleFor(x => x.Sms)
                .SetValidator(new SmsNotificationValidator()!)
                .When(x => x.Sms != null);
        }
    }
}
