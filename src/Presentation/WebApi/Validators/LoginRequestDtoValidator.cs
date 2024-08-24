using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation;
using Presentation.WebApi.Models.Authentication;

namespace Presentation.WebApi.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    private readonly IUserRepository _userRepository;
    internal const string IncorrectLoginDetails = "Incorrect login details";

    public LoginRequestDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => new { x.Username, x.Password })
            .Cascade(CascadeMode.Stop)
            .WithDisplayName($"{nameof(Username)} or {nameof(Password)}")
            .MustAsync(async (x, _) => await UserExistByUsername(x.Username))
            .WithMessage(IncorrectLoginDetails)
            .MustAsync(async (x, _) => await CorrectPassword(x.Username, x.Password))
            .WithMessage(IncorrectLoginDetails);
    }

    private Task<bool> UserExistByUsername(string username)
    {
        return _userRepository.ExistsByUsernameAsync(username);
    }

    private async Task<bool> CorrectPassword(string username, string password)
    {
        var exists = await _userRepository.ExistsByUsernameAsync(username);
        if (!exists)
        {
            return false;
        }

        var user = await _userRepository.GetByUsernameAsync(username);
        return user.Authenticate(password);
    }
}