namespace Core.Constants;

public static class ApiUrls
{
    public static class Authentication
    {
        public const string Login = "/auth/login";
        public const string Register = "/auth/register";
        public const string RequestResetPassword = "/auth/request-reset-password";
        public const string ResetPassword = "/auth/reset-password";
    }

    public static class User
    {
        public const string Get = "/user";
        public const string Update = "/user";
        public const string ChangePassword = "/user/password";

    }
}