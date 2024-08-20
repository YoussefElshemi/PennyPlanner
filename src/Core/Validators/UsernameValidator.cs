using Core.Constants;
using Core.Extensions;
using Core.ValueObjects;
using FluentValidation;

namespace Core.Validators;

public class UsernameValidator : AbstractValidator<string>
{
    internal const int MinLength = 3;
    internal const int MaxLength = 50;


    public UsernameValidator(string displayName = nameof(Username))
    {
        RuleFor(username => username)
            .WithDisplayName(displayName)
            .NotEmpty()
            .MinimumLength(MinLength)
            .MaximumLength(MaxLength)
            .Matches(RegexPatterns.UsernameRegexPattern);
    }
}