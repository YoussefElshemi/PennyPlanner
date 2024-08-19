using Core.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Core.Validators;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator = new();

    [Fact]
    public void Validate_PasswordIsEmpty_ReturnsError()
    {
        // Act
        var password = string.Empty;

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public void Validate_LessThanMinimumPasswordLength_ReturnsError()
    {
        // Act
        var password = string.Concat(Enumerable.Repeat("a", PasswordValidator.MinLength - 1));

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MinimumLengthValidator");
    }

    [Fact]
    public void Validate_MoreThanMaximumPasswordLength_ReturnsError()
    {
        // Act
        var password = string.Concat(Enumerable.Repeat("a", PasswordValidator.MaxLength + 1));

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MaximumLengthValidator");
    }

    [Fact]
    public void Validate_DoesNotContainLowercaseLetter_ReturnsError()
    {
        // Act
        var password = "A0!A0!A0!";

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.LowercaseErrorMessage);
    }

    [Fact]
    public void Validate_DoesNotContainUppercaseLetter_ReturnsError()
    {
        // Act
        var password = "a0!a0!a0!";

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.UppercaseErrorMessage);
    }

    [Fact]
    public void Validate_DoesNotContainDigit_ReturnsError()
    {
        // Act
        var password = "Aa!Aa!Aa!";

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.DigitErrorMessage);
    }

    [Fact]
    public void Validate_DoesNotContainSpecialCharacter_ReturnsError()
    {
        // Act
        var password = "Aa0Aa0Aa0";

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.SpecialCharacterErrorMessage);
    }

    [Fact]
    public void Validate_MeetsRequirements_IsValid()
    {
        // Act
        var password = FakePassword.Valid;

        // Arrange
        var result = _validator.TestValidate(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}