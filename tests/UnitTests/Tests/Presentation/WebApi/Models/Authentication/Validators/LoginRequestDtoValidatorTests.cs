using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Authentication.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Authentication.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.Models.Authentication.Validators;

public class LoginRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IAuthenticationService> _mockAuthenticationService = new();
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly LoginRequestDtoValidator _validator;

    public LoginRequestDtoValidatorTests()
    {
        _validator = new LoginRequestDtoValidator(_mockAuthenticationService.Object,
            _mockUserRepository.Object);
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
            .WithErrorMessage(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_IncorrectPassword_Throws()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var user = FakeUser.CreateValid(Fixture);

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(false);

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
            .WithErrorMessage(LoginRequestDtoValidator.IncorrectLoginDetailsErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var loginRequestDto = FakeLoginRequestDto.CreateValid();
        var user = FakeUser.CreateValid(Fixture);

        _mockAuthenticationService
            .Setup(x => x.Authenticate(It.IsAny<User>(), It.IsAny<Password>()))
            .Returns(true);

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