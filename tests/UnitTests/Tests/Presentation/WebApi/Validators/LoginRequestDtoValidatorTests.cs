using Core.Interfaces.Repositories;
using Core.Services;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class LoginRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly LoginRequestDtoValidator _validator;

    public LoginRequestDtoValidatorTests()
    {
        _validator = new LoginRequestDtoValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_Throws()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(loginRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(LoginRequestDtoValidator.IncorrectLoginDetails);
    }

    [Fact]
    public async Task ValidateAsync_IncorrectPassword_Throws()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid() with
        {
            Password = "wrong"
        };
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword("password", Convert.FromBase64String(user.PasswordSalt.ToString())))
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(loginRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(LoginRequestDtoValidator.IncorrectLoginDetails);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var user = FakeUser.CreateValid(Fixture);
        user = user with
        {
            PasswordHash = new PasswordHash(AuthenticationService.HashPassword(loginRequestDto.Password, Convert.FromBase64String(user.PasswordSalt.ToString())))
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(loginRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}