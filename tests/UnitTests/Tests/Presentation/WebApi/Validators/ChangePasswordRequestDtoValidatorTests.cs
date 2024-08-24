using FluentValidation.TestHelper;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class ChangePasswordRequestDtoValidatorTests : BaseTestClass
{
    private readonly ChangePasswordRequestDtoValidator _validator;

    public ChangePasswordRequestDtoValidatorTests()
    {
        var currentUser = FakeUser.CreateValid(Fixture);
        _validator = new ChangePasswordRequestDtoValidator(currentUser);
    }

    [Fact]
    public async Task ValidateAsync_InvalidPassword_ReturnsError()
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
    public async Task ValidateAsync_PasswordsDoNotMatch_ReturnsError()
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
    public async Task ValidateAsync_PasswordsDidNotChange_ReturnsError()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ChangePasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid() with
        {
            Password = string.Join("", FakePassword.Valid.ToCharArray().Reverse()),
            ConfirmPassword = string.Join("", FakePassword.Valid.ToCharArray().Reverse())
        };

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}