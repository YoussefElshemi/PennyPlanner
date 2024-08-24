using Core.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Tests.Core.Validators;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_PasswordIsEmpty_ReturnsError()
    {
        // Arrange
        var password = string.Empty;

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_LessThanMinimumPasswordLength_ReturnsError()
    {
        // Arrange
        var password = string.Concat(Enumerable.Repeat("a", PasswordValidator.MinLength - 1));

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MinimumLengthValidator");
    }

    [Fact]
    public async Task ValidateAsync_MoreThanMaximumPasswordLength_ReturnsError()
    {
        // Arrange
        var password = string.Concat(Enumerable.Repeat("a", PasswordValidator.MaxLength + 1));

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MaximumLengthValidator");
    }

    [Fact]
    public async Task ValidateAsync_DoesNotContainLowercaseLetter_ReturnsError()
    {
        // Arrange
        const string password = "A0!A0!A0!";

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.LowercaseErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_DoesNotContainUppercaseLetter_ReturnsError()
    {
        // Arrange
        const string password = "a0!a0!a0!";

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.UppercaseErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_DoesNotContainDigit_ReturnsError()
    {
        // Arrange
        const string password = "Aa!Aa!Aa!";

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.DigitErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_DoesNotContainSpecialCharacter_ReturnsError()
    {
        // Arrange
        const string password = "Aa0Aa0Aa0";

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(PasswordValidator.SpecialCharacterErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_MeetsRequirements_IsValid()
    {
        // Arrange
        const string password = FakePassword.Valid;

        // Act
        var result = await _validator.TestValidateAsync(password);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}