namespace Core.Constants;

public static class RegexPatterns
{
    public const string UsernameRegexPattern = @"^[a-zA-Z0-9_\-\.]+$";
    public const string PasswordRegexPattern = @"^[a-zA-Z0-9!@#$%^&*()_+=\[{\]};:<>|./?,-]*$";
}