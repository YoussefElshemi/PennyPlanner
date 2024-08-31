namespace Presentation.Constants;

public static class SwaggerSummaries
{
    public static class Authentication
    {
        public const string Login = "Authenticates a user and provides a JWT token.";
        public const string Register = "Registers a new user.";
        public const string RefreshToken = "Refreshes the access token using a refresh token.";
        public const string RevokeRefreshToken = "Revokes a refresh token to prevent further use.";
        public const string RequestPasswordReset = "Requests a password reset for the specified email address.";
        public const string ResetPassword = "Resets the password using a password reset token.";
    }

    public static class User
    {
        public const string Get = "Retrieves the profile information of the currently authenticated user.";
        public const string Update = "Updates the current user's profile information.";
        public const string ChangePassword = "Updates the current user's password.";
    }

    public static class UserManagement
    {
        public const string GetUsers = "Retrieves a paginated list of users.";
        public const string GetUsersSortableFields = "Retrieves fields which can be used to sort users.";
        public const string GetUsersSearchableFields = "Retrieves fields which can be used to search users.";
        public const string DeleteUser = "Deletes a user with the specified userId.";
        public const string UpdateUser = "Updates the profile information of a user with the specified userId.";
    }

    public static class Emails
    {
        public const string GetEmails = "Retrieves a paginated list of emails.";
    }
}