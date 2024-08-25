using Core.Validators;
using FluentValidation;

namespace Presentation.WebApi.Models.Authentication.Validators;

public class RequestResetPasswordRequestDtoValidator : AbstractValidator<RequestResetPasswordRequestDto>
{
    public RequestResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator());
    }
}