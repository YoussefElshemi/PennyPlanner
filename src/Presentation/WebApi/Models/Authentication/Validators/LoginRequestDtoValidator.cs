using System.Net;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FluentValidation;

namespace Presentation.WebApi.Models.Authentication.Validators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    internal const string IncorrectLoginDetails = "Incorrect login details";
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;

    public LoginRequestDtoValidator(IAuthenticationService authenticationService,
        IUserRepository userRepository)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;

        RuleFor(x => new { x.Username, x.Password })
            .Cascade(CascadeMode.Stop)
            .WithDisplayName($"{nameof(Username)} or {nameof(Password)}")
            .MustAsync(async (x, _) => await UserExistByUsername(x.Username))
            .WithErrorCode(HttpStatusCode.Unauthorized.ToString())
            .WithMessage(IncorrectLoginDetails)
            .MustAsync(async (x, _) => await CorrectPassword(x.Username, x.Password))
            .WithErrorCode(HttpStatusCode.Unauthorized.ToString())
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
        return _authenticationService.Authenticate(user, new Password(password));
    }
}