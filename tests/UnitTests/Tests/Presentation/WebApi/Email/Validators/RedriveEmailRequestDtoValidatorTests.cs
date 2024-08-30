using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Email.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Email.Models.Request;

namespace UnitTests.Tests.Presentation.WebApi.Email.Validators;

public class RedriveEmailRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IEmailRepository> _mockEmailRepository = new();
    private readonly RedriveEmailRequestDtoValidator _validator;

    public RedriveEmailRequestDtoValidatorTests()
    {
        _validator = new RedriveEmailRequestDtoValidator(_mockEmailRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_EmailIdIsEmpty_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRedriveEmailRequestDto.CreateValid(Fixture) with
        {
            EmailId = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailId)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_EmailDoesNotExist_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRedriveEmailRequestDto.CreateValid(Fixture);
        _mockEmailRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailId)
            .WithErrorMessage(RedriveEmailRequestDtoValidator.EmailDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_EmailIsProcessed_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRedriveEmailRequestDto.CreateValid(Fixture);
        var emailMessage = FakeEmailMessage.CreateValid(Fixture) with
        {
            IsProcessed = new IsProcessed(true)
        };

        _mockEmailRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockEmailRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(emailMessage);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailId)
            .WithErrorMessage(RedriveEmailRequestDtoValidator.EmailAlreadyProcessedErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRedriveEmailRequestDto.CreateValid(Fixture);
        var emailMessage = FakeEmailMessage.CreateValid(Fixture) with
        {
            IsProcessed = new IsProcessed(false)
        };

        _mockEmailRepository
            .Setup(x => x.ExistsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockEmailRepository
            .Setup(x => x.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(emailMessage);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}