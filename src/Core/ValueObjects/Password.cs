using Core.Validators;
using FluentValidation;

namespace Core.ValueObjects;

public record struct Password
{
    private string Value { get; init; }

    public Password(string password)
    {
        new PasswordValidator().ValidateAndThrow(password);
        Value = password;
    }

    public static implicit operator string(Password password)
    {
        return password.Value;
    }
}