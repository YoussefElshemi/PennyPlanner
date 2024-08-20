using Core.Validators;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .SetValidator(new UsernameValidator());

        RuleFor(x => x.Password)
            .SetValidator(new PasswordValidator());
    }
}