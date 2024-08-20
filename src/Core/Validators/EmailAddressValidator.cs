using Core.Extensions;
using Core.ValueObjects;
using EmailValidation;
using FluentValidation;

namespace Core.Validators;

public class EmailAddressValidator : AbstractValidator<string>
{
    public EmailAddressValidator(string displayName = nameof(EmailAddress))
    {
        RuleFor(emailAddress => emailAddress)
            .WithDisplayName(displayName)
            .NotEmpty()
            .Must(x => EmailValidator.Validate(x));
    }
}