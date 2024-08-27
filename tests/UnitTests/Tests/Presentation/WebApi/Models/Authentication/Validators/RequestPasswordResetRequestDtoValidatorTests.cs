using FluentValidation.TestHelper;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.Models.Authentication.Validators;

public class RequestPasswordResetRequestDtoValidatorTests
{
    private readonly RequestPasswordResetRequestDtoValidator _validator = new();

    [Fact]
    public async Task ValidateAsync_InvalidEmailAddress_ReturnsError()
    {
        // Arrange
        var requestPasswordResetRequestDto = FakeRequestPasswordResetRequestDto.CreateValid() with
        {
            EmailAddress = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(requestPasswordResetRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var requestPasswordResetRequestDto = FakeRequestPasswordResetRequestDto.CreateValid();

        // Act
        var result = await _validator.TestValidateAsync(requestPasswordResetRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}