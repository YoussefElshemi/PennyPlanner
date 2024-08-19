using Core.Validators;
using FluentValidation;

namespace Core.ValueObjects;

public record struct AuthenticationToken
{
    private string Value { get; init; }

    public AuthenticationToken(string authenticationToken)
    {
        ArgumentNullException.ThrowIfNull(authenticationToken);
        Value = authenticationToken;
    }

    public static implicit operator string(AuthenticationToken authenticationToken)
    {
        return authenticationToken.Value;
    }
}