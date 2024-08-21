using FluentValidation.TestHelper;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class ChangePasswordRequestDtoValidatorTests
{
    private readonly ChangePasswordRequestDtoValidator _validator = new();

    [Fact]
    public async Task Validate_InvalidPassword_ReturnsError()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid() with
        {
            Password = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task Validate_PasswordsDoNotMatch_ReturnsError()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid() with
        {
            Password = FakePassword.Valid,
            ConfirmPassword = string.Join("", FakePassword.Valid.ToCharArray().Reverse())
        };

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ChangePasswordRequestDtoValidator.ConfirmPasswordErrorMessage);
    }

    [Fact]
    public async Task Validate_ValidRequest_IsValid()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}