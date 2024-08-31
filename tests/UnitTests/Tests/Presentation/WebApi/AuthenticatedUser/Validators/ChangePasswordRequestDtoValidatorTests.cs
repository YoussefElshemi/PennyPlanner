using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.AuthenticatedUser.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.AuthenticatedUser.Validators;

public class ChangePasswordRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IAuthenticationService> _mockAuthenticationService;
    private readonly ChangePasswordRequestDtoValidator _validator;

    public ChangePasswordRequestDtoValidatorTests()
    {
        var user = FakeUser.CreateValid(Fixture);
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        _validator = new ChangePasswordRequestDtoValidator(_mockAuthenticationService.Object, user);
    }

    [Fact]
    public async Task ValidateAsync_InvalidPassword_ReturnsError()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid() with
        {
            Password = ""
        };

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true);

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

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true);

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

        _mockAuthenticationService
            .SetupSequence(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true)
            .Returns(true);

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ChangePasswordRequestDtoValidator.PasswordDidNotChangeErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_PasswordIncorrect_ReturnsError()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        _mockAuthenticationService
            .SetupSequence(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(false)
            .Returns(true);


        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(ChangePasswordRequestDtoValidator.PasswordIncorrectErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var changePasswordRequestDto = FakeChangePasswordRequestDto.CreateValid();

        _mockAuthenticationService
            .SetupSequence(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true)
            .Returns(false);

        // Act
        var result = await _validator.TestValidateAsync(changePasswordRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}