using FluentValidation.TestHelper;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class LoginRequestDtoValidatorTests
{
    private readonly LoginRequestDtoValidator _validator = new();

    [Fact]
    public void Validate_InvalidUsername_ReturnsError()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid() with
        {
            Username = ""
        };

        // Act
        var result = _validator.TestValidate(loginRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public void Validate_InvalidPassword_ReturnsError()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid() with
        {
            Password = ""
        };

        // Act
        var result = _validator.TestValidate(loginRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();

        // Act
        var result = _validator.TestValidate(loginRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}