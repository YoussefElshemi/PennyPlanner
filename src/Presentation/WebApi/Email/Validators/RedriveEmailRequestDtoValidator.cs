using System.Net;
using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Email.Models;

namespace Presentation.WebApi.Email.Validators;

public class RedriveEmailRequestDtoValidator : AbstractValidator<RedriveEmailRequestDto>
{
    internal const string EmailDoesNotExistErrorMessage = "Email does not exist";
    internal const string EmailAlreadyProcessedErrorMessage = "Email has already been processed.";

    public RedriveEmailRequestDtoValidator(IEmailRepository emailRepository)
    {
        RuleFor(x => x.EmailId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await emailRepository.ExistsAsync(new EmailId(x)))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(EmailDoesNotExistErrorMessage)
            .MustAsync(async (x, _) => (await emailRepository.GetAsync(new EmailId(x))).IsProcessed == false)
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(EmailAlreadyProcessedErrorMessage);
    }
}