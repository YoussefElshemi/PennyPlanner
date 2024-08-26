using Core.Validators;
using FluentValidation;
using Presentation.WebApi.Authentication.Models.Requests;

namespace Presentation.WebApi.Authentication.Validators;

public class RequestResetPasswordRequestDtoValidator : AbstractValidator<RequestResetPasswordRequestDto>
{
    public RequestResetPasswordRequestDtoValidator()
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator());
    }
}