namespace Presentation.Constants;

public static class ApiRoutes
{
    public static class Authentication
    {
        public const string Login = "/auth/login";
        public const string Register = "/auth/register";
        public const string RefreshToken = "/auth/refresh-token";
        public const string RevokeRefreshToken = "/auth/revoke-refresh-token";
        public const string RequestResetPassword = "/auth/request-reset-password";
        public const string ResetPassword = "/auth/reset-password";
    }

    public static class User
    {
        public const string Get = "/user";
        public const string Update = "/user";
        public const string ChangePassword = "/user/password";
    }

    public static class UserManagement
    {
        public const string GetUsers = "/user-management/users";
        public const string GetUsersSearchableFields = "/user-management/users/searchable-fields";
        public const string GetUsersSortableFields = "/user-management/users/sortable-fields";
        public const string DeleteUser = "/user-management/users/{userId:guid}";
        public const string UpdateUser = "/user-management/users/{userId:guid}";
    }
}