using Core.Validators;
using FluentValidation;
using Presentation.WebApi.Authentication.Models.Requests;

namespace Presentation.WebApi.Authentication.Validators;

public class RequestPasswordResetRequestDtoValidator : AbstractValidator<RequestPasswordResetRequestDto>
{
    public RequestPasswordResetRequestDtoValidator()
    {
        RuleFor(x => x.EmailAddress)
            .SetValidator(new EmailAddressValidator());
    }
}