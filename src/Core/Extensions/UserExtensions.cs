using Core.Models;

namespace Core.Extensions;

public static class UserExtensions
{
    public static bool Authenticate(this User user, string password)
    {
        return user.PasswordHash == password.Md5Hash();
    }
}