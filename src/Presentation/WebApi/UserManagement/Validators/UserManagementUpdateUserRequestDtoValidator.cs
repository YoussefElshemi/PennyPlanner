using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation;
using Presentation.WebApi.UserManagement.Models.Requests;

namespace Presentation.WebApi.UserManagement.Validators;

public class UserManagementUpdateUserRequestDtoValidator : AbstractValidator<UserManagementUpdateUserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotUpdateAdminErrorMessage = $"{nameof(User)} is admin and cannot be updated.";
    internal const string InvalidUserRoleErrorMessage = $"{nameof(UserRole)} is invalid.";
    internal static readonly Func<string, string> FieldDidNotUpdateErrorMessage = field => $"{field} is the same as the current value.";

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
            .WithMessage(CanNotUpdateAdminErrorMessage)
            .DependentRules(() =>
            {
                RuleFor(x => x.UserRole)
                    .IsEnumName(typeof(UserRole), false)
                    .WithMessage(InvalidUserRoleErrorMessage)
                    .When(x => !string.IsNullOrWhiteSpace(x.UserRole))
                    .DependentRules(() =>
                    {
                        RuleFor(x => new { x.UserId, x.UserRole })
                            .MustAsync(async (x, _) =>
                                !x.UserRole.Equals((await userRepository.GetByIdAsync(x.UserId)).UserRole.ToString(), StringComparison.OrdinalIgnoreCase))
                            .WithErrorCode(HttpStatusCode.Conflict.ToString())
                            .WithMessage(FieldDidNotUpdateErrorMessage(nameof(UserRole)))
                            .MustAsync(async (x, _) =>
                                (await userRepository.GetByIdAsync(x.UserId)).UserRole != UserRole.Admin)
                            .WithMessage(CanNotUpdateAdminErrorMessage)
                            .When(x => !string.IsNullOrWhiteSpace(x.UserRole));
                    });
            });
    }

    public UserManagementUpdateUserRequestDtoValidator()
    {
    }
}