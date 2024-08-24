using Core.Enums;
using Core.Interfaces.Repositories;
using FluentValidation;
using Presentation.WebApi.Models.UserManagement;

namespace Presentation.WebApi.Validators.UserManagement;

public class DeleteUserRequestDtoValidator : AbstractValidator<GetUserRequestDto>
{
    internal const string UserDoesNotExistErrorMessage = $"{nameof(User)} does not exist.";
    internal const string CanNotDeleteAdminErrorMessage = $"{nameof(User)} is admin and cannot be deleted.";

    public  DeleteUserRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (x, _) => await userRepository.ExistsByIdAsync(x))
            .WithMessage(UserDoesNotExistErrorMessage)
            .MustAsync(async (x, _) => (await userRepository.GetByIdAsync(x)).UserRole != UserRole.Admin)
            .WithMessage(CanNotDeleteAdminErrorMessage);
    }
}