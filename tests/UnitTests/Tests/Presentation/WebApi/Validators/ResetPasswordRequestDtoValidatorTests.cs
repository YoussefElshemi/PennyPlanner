using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class ResetPasswordRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IPasswordResetRepository> _mockPasswordResetRepository;
    private readonly ResetPasswordRequestDtoValidator _validator;

    public ResetPasswordRequestDtoValidatorTests()
    {
        var currentUser = FakeUser.CreateValid(Fixture);
        _mockPasswordResetRepository = new Mock<IPasswordResetRepository>();
        _validator = new ResetPasswordRequestDtoValidator(_mockPasswordResetRepository.Object, currentUser);
    }

    [Fact]
    public async Task Validate_PasswordResetTokenDoesNotExist_ReturnsError()
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
    public async Task Validate_PasswordResetTokenAlreadyUsed_ReturnsError()
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
    public async Task Validate_PasswordInvalid_ReturnsError()
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
    public async Task Validate_PasswordDoesNotMatch_ReturnsError()
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
    public async Task Validate_PasswordDidNotChange_ReturnsError()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture);
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
            .WithErrorMessage(ResetPasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
    }

    [Fact]
    public async Task Validate_ValidRequest_IsValid()
    {
        // Arrange
        var resetPasswordRequestDto = FakeResetPasswordRequestDto.CreateValid(Fixture) with
        {
            Password = string.Join("", FakePassword.Valid.ToCharArray().Reverse()),
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
        result.ShouldNotHaveAnyValidationErrors();
    }
}