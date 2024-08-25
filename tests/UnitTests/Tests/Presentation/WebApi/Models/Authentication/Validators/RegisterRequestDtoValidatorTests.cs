using Core.Interfaces.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Models.Authentication.Validators;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Authentication;

namespace UnitTests.Tests.Presentation.WebApi.Models.Authentication.Validators;

public class RegisterRequestDtoValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly RegisterRequestDtoValidator _validator;

    public RegisterRequestDtoValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new RegisterRequestDtoValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_InvalidUsername_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            Username = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username);
    }

    [Fact]
    public async Task ValidateAsync_InvalidPassword_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            Password = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public async Task ValidateAsync_InvalidEmailAddress_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            EmailAddress = ""
        };

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress);
    }

    [Fact]
    public async Task ValidateAsync_PasswordsDoNotMatch_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            Password = FakePassword.Valid,
            ConfirmPassword = string.Join("", FakePassword.Valid.ToCharArray().Reverse())
        };

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(RegisterRequestDtoValidator.ConfirmPasswordErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UsernameAlreadyExists_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            Username = FakeUsername.Valid
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(RegisterRequestDtoValidator.UsernameTakenErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_EmailAddressAlreadyExists_ReturnsError()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid() with
        {
            EmailAddress = FakeEmailAddress.Valid
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(RegisterRequestDtoValidator.EmailAddressInUseErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var registerRequestDto = FakeRegisterRequestDto.CreateValid();
        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(registerRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}