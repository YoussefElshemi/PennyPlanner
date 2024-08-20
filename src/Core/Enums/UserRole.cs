using System.ComponentModel;

namespace Core.Enums;

public enum UserRole
{
    [Description("User")]
    User = 1,
    [Description("Admin")]
    Admin
}