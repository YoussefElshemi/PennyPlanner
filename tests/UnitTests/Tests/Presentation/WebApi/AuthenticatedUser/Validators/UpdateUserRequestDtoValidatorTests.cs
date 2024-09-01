using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.AuthenticatedUser.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.User.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.AuthenticatedUser.Validators;

public class UpdateUserRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UpdateUserRequestDtoValidator _validator;

    public UpdateUserRequestDtoValidatorTests()
    {
        var user = FakeUser.CreateValid(Fixture);
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new UpdateUserRequestDtoValidator(_mockUserRepository.Object, user);
    }

    [Fact]
    public async Task ValidateAsync_EmailAddressIsCurrentEmailAddress_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress)
            .WithErrorMessage(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage(nameof(EmailAddress)));
    }

    [Fact]
    public async Task ValidateAsync_EmailAddressAlreadyExists_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null,
            EmailAddress = "new@email.com"
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.EmailAddressInUseErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UsernameIsCurrentEmailAddress_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            EmailAddress = null
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username)
            .WithErrorMessage(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage(nameof(Username)));
    }

    [Fact]
    public async Task ValidateAsync_UsernameAlreadyExists_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = "new",
            EmailAddress = null
        };

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.UsernameInUseErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = "new",
            EmailAddress = "new@email.com"
        };

        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}