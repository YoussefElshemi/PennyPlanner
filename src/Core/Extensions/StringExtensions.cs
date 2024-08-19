using System.Text;

namespace Core.Extensions;

public static class StringExtensions
{
    public static string Md5Hash(this string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes);
    }
}