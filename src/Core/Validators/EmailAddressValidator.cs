using Core.Extensions;
using Core.ValueObjects;
using EmailValidation;
using FluentValidation;

namespace Core.Validators;

public class EmailAddressValidator : AbstractValidator<string>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress)
            .WithDisplayName(nameof(EmailAddress))
            .NotEmpty()
            .Must(x => EmailValidator.Validate(x));
    }
}