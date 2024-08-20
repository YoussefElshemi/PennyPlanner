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
        // Act
        var username = string.Empty;

        // Arrange
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task Validate_LessThanMinimumUsernameLength_ReturnsError()
    {
        // Act
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MinLength - 1));

        // Arrange
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MinimumLengthValidator");
    }

    [Fact]
    public async Task Validate_MoreThanMaximumUsernameLength_ReturnsError()
    {
        // Act
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MaxLength + 1));

        // Arrange
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MaximumLengthValidator");
    }

    [Fact]
    public async Task Validate_ContainsIllegalCharacters_ReturnsError()
    {
        // Act
        var username = "!llegalUsername";

        // Arrange
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("RegularExpressionValidator");
    }

    [Fact]
    public async Task Validate_MeetsRequirements_IsValid()
    {
        // Act
        var username = FakeUsername.Valid;

        // Arrange
        var result = await _validator.TestValidateAsync(username);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}