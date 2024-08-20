using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Validators;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    internal const string IncorrectLoginDetails = "Incorrect login details";

    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Username)
            .WithDisplayName(nameof(Username))
            .SetValidator(new UsernameValidator());

        RuleFor(x => x.Password)
            .WithDisplayName(nameof(Username))
            .SetValidator(new PasswordValidator());
    }

    public LoginRequestDtoValidator(IUserRepository userRepository)
    {
        RuleFor(x => new { x.Username, x.Password })
            .Cascade(CascadeMode.Stop)
            .WithDisplayName($"{nameof(Username)} and {nameof(Password)}")
            .MustAsync(async (x, _) => await UserExistByUsername(userRepository, x.Username))
            .WithMessage(IncorrectLoginDetails)
            .MustAsync(async (x, _) => await CorrectPassword(userRepository, x.Username, x.Password))
            .WithMessage(IncorrectLoginDetails);
    }


    private static async Task<bool> UserExistByUsername(IUserRepository userRepository, string username)
    {
        return await userRepository.ExistsByUsernameAsync(username);
    }

    private static async Task<bool> CorrectPassword(IUserRepository userRepository, string username, string password)
    {
        var exists = await userRepository.ExistsByUsernameAsync(username);
        if (!exists)
        {
            return false;
        }

        var user = await userRepository.GetByUsernameAsync(username);
        if (user is null)
        {
            return false;
        }

        return user.Authenticate(password);
    }
}