using Core.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Tests.Core.Validators;

public class UsernameValidatorTests
{
    private readonly UsernameValidator _validator = new();

    [Fact]
    public async Task Validate_UsernameIsEmpty_ReturnsError()
    {
        // Arrange
        var username = string.Empty;

        // Act
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task Validate_LessThanMinimumUsernameLength_ReturnsError()
    {
        // Arrange
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MinLength - 1));

        // Act
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MinimumLengthValidator");
    }

    [Fact]
    public async Task Validate_MoreThanMaximumUsernameLength_ReturnsError()
    {
        // Arrange
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MaxLength + 1));

        // Act
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MaximumLengthValidator");
    }

    [Fact]
    public async Task Validate_ContainsIllegalCharacters_ReturnsError()
    {
        // Arrange
        var username = "!llegalUsername";

        // Act
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("RegularExpressionValidator");
    }

    [Fact]
    public async Task Validate_MeetsRequirements_IsValid()
    {
        // Arrange
        var username = FakeUsername.Valid;

        // Act
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}