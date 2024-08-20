using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
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
    public async Task Validate_UserDoesNotExist_Throws()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();

        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(loginRequestDto.Password.Md5Hash())
        };

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
    public async Task Validate_IncorrectPassword_Throws()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid() with
        {
            Password = "password"
        };
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash("wrong".Md5Hash())
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
    public async Task Validate_ValidRequest_IsValid()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var user = FakeUser.CreateValid(Fixture) with
        {
            PasswordHash = new PasswordHash(loginRequestDto.Password.Md5Hash())
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