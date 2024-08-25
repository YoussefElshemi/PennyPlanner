using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using FluentValidation;

namespace Presentation.WebApi.Models.UserManagement.Validators;

public class DeleteUserRequestDtoValidator : AbstractValidator<GetUserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotDeleteAdminErrorMessage = $"{nameof(User)} is admin and cannot be deleted.";

    public DeleteUserRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(UserDoesNotExistErrorMessage)
            .MustAsync(async (x, _) => (await userRepository.GetByIdAsync(x)).UserRole != UserRole.Admin)
            .WithErrorCode(HttpStatusCode.Forbidden.ToString())
            .WithMessage(CanNotDeleteAdminErrorMessage);
    }
}