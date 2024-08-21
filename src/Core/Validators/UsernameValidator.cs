using Core.Constants;
using Core.Extensions;
using Core.ValueObjects;
using FluentValidation;

namespace Core.Validators;

public class UsernameValidator : AbstractValidator<string>
{
    internal const int MinLength = 3;
    internal const int MaxLength = 50;

    public UsernameValidator()
    {
        RuleFor(username => username)
            .WithDisplayName(nameof(Username))
            .NotEmpty()
            .MinimumLength(MinLength)
            .MaximumLength(MaxLength)
            .Matches(RegexPatterns.UsernameRegexPattern);
    }
}