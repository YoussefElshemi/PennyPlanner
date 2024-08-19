using Core.Validators;
using FluentValidation.TestHelper;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.Core.Validators;

public class UsernameValidatorTests
{
    private readonly UsernameValidator _validator = new();

    [Fact]
    public void Validate_UsernameIsEmpty_ReturnsError()
    {
        // Act
        var username = string.Empty;

        // Arrange
        var result = _validator.TestValidate(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public void Validate_LessThanMinimumUsernameLength_ReturnsError()
    {
        // Act
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MinLength - 1));

        // Arrange
        var result = _validator.TestValidate(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MinimumLengthValidator");
    }

    [Fact]
    public void Validate_MoreThanMaximumUsernameLength_ReturnsError()
    {
        // Act
        var username = string.Concat(Enumerable.Repeat("a", UsernameValidator.MaxLength + 1));

        // Arrange
        var result = _validator.TestValidate(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("MaximumLengthValidator");
    }

    [Fact]
    public void Validate_ContainsIllegalCharacters_ReturnsError()
    {
        // Act
        var username = "!llegalUsername";

        // Arrange
        var result = _validator.TestValidate(username);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("RegularExpressionValidator");
    }

    [Fact]
    public void Validate_MeetsRequirements_IsValid()
    {
        // Act
        var username = FakeUsername.Valid;

        // Arrange
        var result = _validator.TestValidate(username);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}