using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.Models.Authentication.Validators;

public class ResetPasswordRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IAuthenticationService> _mockAuthenticationService;
    private readonly Mock<IPasswordResetRepository> _mockPasswordResetRepository;
    private readonly ResetPasswordRequestDtoValidator _validator;

    public ResetPasswordRequestDtoValidatorTests()
    {
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        _mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        _validator = new ResetPasswordRequestDtoValidator(_mockAuthenticationService.Object,
            _mockPasswordResetRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_PasswordResetTokenIsEmpty_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture) with
        {
            PasswordResetToken = string.Empty
        };

        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_PasswordResetTokenDoesNotExist_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);
        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ResetPasswordRequestDtoValidator.PasswordResetTokenNotFoundErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasswordResetTokenAlreadyUsed_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);
        var passwordReset = FakePasswordReset.CreateValid(Fixture) with
        {
            IsUsed = new IsUsed(true)
        };

        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockPasswordResetRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(passwordReset);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ResetPasswordRequestDtoValidator.PasswordResetTokenAlreadyUsedErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasswordInvalid_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture) with
        {
            Password = "",
            ConfirmPassword = ""
        };

        var passwordReset = FakePasswordReset.CreateValid(Fixture);

        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockPasswordResetRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(passwordReset);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task ValidateAsync_PasswordDoesNotMatch_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture) with
        {
            ConfirmPassword = string.Join("", FakePassword.Valid.ToCharArray().Reverse())
        };

        var passwordReset = FakePasswordReset.CreateValid(Fixture);


        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockPasswordResetRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(passwordReset);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ResetPasswordRequestDtoValidator.ConfirmPasswordErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasswordDidNotChange_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);
        var passwordReset = FakePasswordReset.CreateValid(Fixture);

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true);

        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockPasswordResetRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(passwordReset);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ResetPasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);
        var passwordReset = FakePasswordReset.CreateValid(Fixture) with
        {
            IsUsed = new IsUsed(false)
        };

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(false);

        _mockPasswordResetRepository
            .Setup(x => x.ExistsAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        _mockPasswordResetRepository
            .Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(passwordReset);

        // Act
        var result = await _validator.TestValidateAsync(resetPasswordRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}