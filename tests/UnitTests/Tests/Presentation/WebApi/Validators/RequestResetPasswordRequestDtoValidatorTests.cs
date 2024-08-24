using FluentValidation.TestHelper;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class RequestResetPasswordRequestDtoValidatorTests
{
    private readonly RequestResetPasswordRequestDtoValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_InvalidEmailAddress_ReturnsError()
    {
        // Arrange
        var requestResetPasswordRequestDto = FakeRequestResetPasswordRequestDto.CreateValid() with
        {
            EmailAddress = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(requestResetPasswordRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var requestResetPasswordRequestDto = FakeRequestResetPasswordRequestDto.CreateValid();

        // Act
        var result = await _validator.TestValidateAsync(requestResetPasswordRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}