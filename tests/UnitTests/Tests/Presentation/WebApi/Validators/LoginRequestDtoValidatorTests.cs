using FluentValidation.TestHelper;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class LoginRequestDtoValidatorTests
{
    private readonly LoginRequestDtoValidator _validator = new();

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