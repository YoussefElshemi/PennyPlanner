using System.Net;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation;
using Presentation.WebApi.UserManagement.Models.Requests;

namespace Presentation.WebApi.UserManagement.Validators;

public class GetUserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";

    public GetUserRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(UserDoesNotExistErrorMessage);
    }
}