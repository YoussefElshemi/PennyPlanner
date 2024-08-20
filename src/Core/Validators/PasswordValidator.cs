using Core.Constants;
using Core.Extensions;
using Core.ValueObjects;
using FluentValidation;

namespace Core.Validators;

public class PasswordValidator : AbstractValidator<string>
{
    internal const int MinLength = 8;
    internal const int MaxLength = 64;

    internal const string LowercaseErrorMessage = $"{nameof(Password)} must contain at least one lowercase letter.";
    internal const string UppercaseErrorMessage = $"{nameof(Password)} must contain at least one uppercase letter.";
    internal const string DigitErrorMessage = $"{nameof(Password)} must contain at least one digit.";
    internal const string SpecialCharacterErrorMessage = $"{nameof(Password)} must contain at least one special character.";

    public PasswordValidator(string displayName = nameof(Password))
    {
        RuleFor(password => password)
            .WithDisplayName(displayName)
            .NotEmpty()
            .MinimumLength(MinLength)
            .MaximumLength(MaxLength)
            .Matches("[a-z]").WithMessage(LowercaseErrorMessage)
            .Matches("[A-Z]").WithMessage(UppercaseErrorMessage)
            .Matches(@"\d").WithMessage(DigitErrorMessage)
            .Matches(@"[\W]").WithMessage(SpecialCharacterErrorMessage)
            .Matches(RegexPatterns.PasswordRegexPattern);
    }
}