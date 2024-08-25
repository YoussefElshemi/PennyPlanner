using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using FluentValidation;
using Presentation.WebApi.Models.UserManagement;

namespace Presentation.WebApi.Validators.UserManagement;

public class UserManagementUpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotUpdateAdminErrorMessage = $"{nameof(User)} is admin and cannot be updated.";

    public UserManagementUpdateUserRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithErrorCode(HttpStatusCode.Conflict.ToString())
            .WithMessage(UserDoesNotExistErrorMessage)
            .MustAsync(async (x, _) => (await userRepository.GetByIdAsync(x)).UserRole != UserRole.Admin)
            .WithErrorCode(HttpStatusCode.Forbidden.ToString())
            .WithMessage(CanNotUpdateAdminErrorMessage);
    }
}