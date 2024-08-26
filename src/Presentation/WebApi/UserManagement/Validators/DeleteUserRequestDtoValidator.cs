using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation;
using Presentation.WebApi.UserManagement.Models.Requests;

namespace Presentation.WebApi.UserManagement.Validators;

public class DeleteUserRequestDtoValidator : AbstractValidator<UserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotDeleteAdminErrorMessage = $"{nameof(User)} is admin and cannot be deleted.";
    internal const string CanNotDeleteSelfErrorMessage = $"Cannot delete currently logged in {nameof(User)}.";

    public DeleteUserRequestDtoValidator(IUserRepository userRepository, User authenticatedUser)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithErrorCode(HttpStatusCode.NotFound.ToString())
            .WithMessage(UserDoesNotExistErrorMessage)
            .Must(x => authenticatedUser.UserId != x)
            .WithErrorCode(HttpStatusCode.Forbidden.ToString())
            .WithMessage(CanNotDeleteSelfErrorMessage)
            .MustAsync(async (x, _) => (await userRepository.GetByIdAsync(x)).UserRole != UserRole.Admin)
            .WithErrorCode(HttpStatusCode.Forbidden.ToString())
            .WithMessage(CanNotDeleteAdminErrorMessage);
    }

    public DeleteUserRequestDtoValidator()
    {
    }
}