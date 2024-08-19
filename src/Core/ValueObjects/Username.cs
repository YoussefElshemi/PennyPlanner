using Core.Validators;
using FluentValidation;

namespace Core.ValueObjects;

public record struct Username
{
    private string Value { get; init; }

    public Username(string username)
    {
        new UsernameValidator().ValidateAndThrow(username);
        Value = username;
    }

    public static implicit operator string(Username username)
    {
        return username.Value;
    }
}