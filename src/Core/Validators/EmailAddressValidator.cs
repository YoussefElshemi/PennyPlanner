using EmailValidation;
using FluentValidation;

namespace Core.Validators;

public class EmailAddressValidator : AbstractValidator<string>
{
    public EmailAddressValidator()
    {
        RuleFor(emailAddress => emailAddress)
            .NotEmpty()
            .Must(x => EmailValidator.Validate(x));
    }
}