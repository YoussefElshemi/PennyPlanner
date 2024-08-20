using Core.Constants;
using FluentValidation;

namespace Core.Validators;

public class UsernameValidator : AbstractValidator<string>
{
    internal const int MinLength = 3;
    internal const int MaxLength = 50;

    public UsernameValidator()
    {
        RuleFor(username => username)
            .NotEmpty()
            .MinimumLength(MinLength)
            .MaximumLength(MaxLength)
            .Matches(RegexPatterns.UsernameRegexPattern);
    }
}