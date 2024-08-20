using System.ComponentModel;

namespace Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        return value
            .GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() is not DescriptionAttribute attribute
            ? string.Empty
            : attribute.Description;
    }
}