using Presentation.WebApi.Models.AuthenticatedUser;
using Presentation.WebApi.Models.Authentication;
using Presentation.WebApi.Models.Common;
using Presentation.WebApi.Models.UserManagement;
using UpdateUserRequestDto = Presentation.WebApi.Models.AuthenticatedUser.UpdateUserRequestDto;
using UserManagementUpdateUserRequestDto = Presentation.WebApi.Models.UserManagement.UpdateUserRequestDto;

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

        public static readonly RequestResetPasswordRequestDto RequestResetPassword = new()
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

        public static readonly UserRequestDto DeleteUser = new UserRequestDto
        {
            UserId = Guid.NewGuid(),
        };

        public static readonly UserManagementUpdateUserRequestDto UpdateUser = new()
        {
            UserId = Guid.NewGuid(),
            Username = "username",
            EmailAddress = "username@mail.com"
        };
    }
}