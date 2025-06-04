using FluentValidation;
using NotificationService.Application.Dtos;

namespace NotificationService.Application.Validators
{
    /// <summary>
    /// FluentValidation validator for SmsNotificationDto
    /// Validates SMS-specific fields like phone numbers and message length
    /// </summary>
    public class SmsNotificationValidator : AbstractValidator<SmsNotificationDto>
    {
        public SmsNotificationValidator()
        {
            // Phone number validation - required and valid format
            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Phone number must be in valid international format (e.g., +1234567890)");

            // Message validation - required with SMS length limits
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("SMS message is required")
                .MaximumLength(160)
                .WithMessage("SMS message cannot exceed 160 characters for single SMS");

            // From number validation - optional but must be valid if provided
            RuleFor(x => x.From)
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("From number must be in valid international format")
                .When(x => !string.IsNullOrEmpty(x.From));
        }
    }
}
