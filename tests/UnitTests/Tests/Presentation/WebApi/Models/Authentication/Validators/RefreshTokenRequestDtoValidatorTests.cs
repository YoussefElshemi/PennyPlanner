using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Models.Authentication.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

namespace UnitTests.Tests.Presentation.WebApi.Models.Authentication.Validators;

public class RefreshTokenRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<ILoginRepository> _mockLoginRepository = new();
    private readonly RefreshTokenRequestDtoValidator _validator;

    public RefreshTokenRequestDtoValidatorTests()
    {
        _validator = new RefreshTokenRequestDtoValidator(_mockLoginRepository.Object,
            MockTimeProvider.Object);
    }

    [Fact]
    public async Task ValidateAsync_RefreshTokenIsEmpty_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture) with
        {
            RefreshToken = string.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_RefreshTokenDoesNotExist_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        _mockLoginRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorMessage(RefreshTokenRequestDtoValidator.RefreshTokenDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_RefreshTokenIsExpired_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            ExpiresAt = new ExpiresAt(Today.AddDays(-1))
        };

        _mockLoginRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockLoginRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(login);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorMessage(RefreshTokenRequestDtoValidator.RefreshTokenIsExpiredErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_RefreshTokenIsRevoked_ReturnsError()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            IsRevoked = new IsRevoked(true)
        };

        _mockLoginRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockLoginRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(login);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.RefreshToken)
            .WithErrorMessage(RefreshTokenRequestDtoValidator.RefreshTokenIsRevokedErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var refreshTokenRequestDto = FakeRefreshTokenRequestDto.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            IsRevoked = new IsRevoked(false)
        };

        _mockLoginRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockLoginRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(login);

        // Act
        var result = await _validator.TestValidateAsync(refreshTokenRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}