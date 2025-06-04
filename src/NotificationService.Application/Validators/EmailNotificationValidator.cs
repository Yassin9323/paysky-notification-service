using FluentValidation;
using NotificationService.Application.Dtos;

namespace NotificationService.Application.Validators
{
    /// <summary>
    /// FluentValidation validator for EmailNotificationDto
    /// Validates email-specific fields like addresses, subject, body
    /// </summary>
    public class EmailNotificationValidator : AbstractValidator<EmailNotificationDto>
    {
        public EmailNotificationValidator()
        {
            // To email validation - required and valid email format
            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("Recipient email is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            // From email validation - optional but must be valid if provided
            RuleFor(x => x.From)
                .EmailAddress()
                .WithMessage("Invalid sender email format")
                .When(x => !string.IsNullOrEmpty(x.From));

            // Subject validation - required with reasonable length
            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage("Email subject is required")
                .MaximumLength(200)
                .WithMessage("Subject cannot exceed 200 characters");

            // Body validation - required with reasonable length
            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage("Email body is required")
                .MaximumLength(10000)
                .WithMessage("Body cannot exceed 10000 characters");

            // CC email validation - all emails must be valid
            RuleForEach(x => x.Cc)
                .EmailAddress()
                .WithMessage("Invalid CC email format")
                .When(x => x.Cc.Any());

            // BCC email validation - all emails must be valid
            RuleForEach(x => x.Bcc)
                .EmailAddress()
                .WithMessage("Invalid BCC email format")
                .When(x => x.Bcc.Any());

            // Limit total recipients (To + CC + BCC) to prevent abuse
            RuleFor(x => x)
                .Must(email => (email.Cc?.Count ?? 0) + (email.Bcc?.Count ?? 0) <= 50)
                .WithMessage("Total CC and BCC recipients cannot exceed 50");
        }
    }
}
