using Core.Enums;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Common.Models.Requests;
using Presentation.WebApi.UserManagement.Models.Requests;
using UpdateUserRequestDto = Presentation.WebApi.AuthenticatedUser.Models.Requests.UpdateUserRequestDto;

namespace Presentation.Constants;

public static class ExampleRequests
{
    public static class Authentication
    {
        public static readonly LoginRequestDto Login = new()
        {
            Username = "username",
            Password = "P@$$w0rd"
        };

        public static readonly RegisterRequestDto Register = new()
        {
            Username = "username",
            Password = "P@$$w0rd",
            ConfirmPassword = "P@$$w0rd",
            EmailAddress = "username@mail.com"
        };

        public static readonly RefreshTokenRequestDto RefreshToken = new()
        {
            RefreshToken = "token"
        };

        public static readonly RefreshTokenRequestDto RevokeRefreshToken = new()
        {
            RefreshToken = "token"
        };

        public static readonly RequestPasswordResetRequestDto RequestPasswordReset = new()
        {
            EmailAddress = "username@mail.com"
        };

        public static readonly ResetPasswordRequestDto ResetPassword = new()
        {
            PasswordResetToken = "token",
            Password = "P@$$w0rd",
            ConfirmPassword = "P@$$w0rd"
        };
    }

    public static class User
    {
        public static readonly UpdateUserRequestDto Update = new()
        {
            Username = "username",
            EmailAddress = "username@mail.com"
        };

        public static readonly ChangePasswordRequestDto ChangePassword = new()
        {
            CurrentPassword = "P@$$w0rd",
            Password = "P@$$w0rd",
            ConfirmPassword = "P@$$w0rd"
        };
    }

    public static class UserManagement
    {
        public static readonly PagedRequestDto GetUsers = new()
        {
            PageNumber = 1,
            PageSize = 10
        };

        public static readonly UserRequestDto DeleteUser = new()
        {
            UserId = Guid.NewGuid()
        };

        public static readonly UserRequestDto GetUser = new()
        {
            UserId = Guid.NewGuid()
        };

        public static readonly UserManagementUpdateUserRequestDto UserManagementUpdateUser = new()
        {
            UserId = Guid.NewGuid(),
            Username = "username",
            EmailAddress = "username@mail.com",
            UserRole = UserRole.User.ToString()
        };
    }

    public static class Emails
    {
        public static readonly PagedRequestDto GetEmails = new()
        {
            PageNumber = 1,
            PageSize = 10
        };
    }
}