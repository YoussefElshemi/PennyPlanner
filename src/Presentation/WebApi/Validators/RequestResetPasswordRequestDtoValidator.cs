using Core.Validators;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class RequestResetPasswordRequestDtoValidator : AbstractValidator<RequestResetPasswordRequestDto>
{
    public RequestResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator());
    }
}