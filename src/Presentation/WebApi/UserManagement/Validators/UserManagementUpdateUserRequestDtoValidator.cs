using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation;
using Presentation.WebApi.UserManagement.Models.Requests;

namespace Presentation.WebApi.UserManagement.Validators;

public class UserManagementUpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotUpdateAdminErrorMessage = $"{nameof(User)} is admin and cannot be updated.";

    public UserManagementUpdateUserRequestDtoValidator(IUserRepository userRepository, User authenticatedUser)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(UserDoesNotExistErrorMessage)
            .MustAsync(async (x, _) =>
                (await userRepository.GetByIdAsync(x)).UserRole != UserRole.Admin ||
                authenticatedUser.UserId == x)
            .WithErrorCode(HttpStatusCode.Forbidden.ToString())
            .WithMessage(CanNotUpdateAdminErrorMessage);
    }

    public UserManagementUpdateUserRequestDtoValidator()
    {
    }
}