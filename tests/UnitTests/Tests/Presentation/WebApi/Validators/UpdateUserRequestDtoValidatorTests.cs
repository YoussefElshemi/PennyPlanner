using AutoFixture;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.WebApi.Validators;

public class UpdateUserRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UpdateUserRequestDtoValidator _validator;
    private readonly User _currentUser;

    public UpdateUserRequestDtoValidatorTests()
    {
        _currentUser = FakeUser.CreateValid(Fixture);
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new UpdateUserRequestDtoValidator(_mockUserRepository.Object, _currentUser);
    }

    [Fact]
    public async Task Validate_NoFieldsProvided_ReturnsError()
    {
        // Act
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null,
            EmailAddress = null
        };

        // Arrange
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.AtLeastOneFieldProvidedErrorMessage);
    }

    [Fact]
    public async Task Validate_EmailAddressIsCurrentEmailAddress_ReturnsError()
    {
        // Act
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null
        };
        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Arrange
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.EmailAddress)
            .WithErrorMessage(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage);
    }

    [Fact]
    public async Task Validate_EmailAddressAlreadyExists_ReturnsError()
    {
        // Act
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = null,
            EmailAddress = "new@email.com"
        };
        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Arrange
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.EmailAddressInUseErrorMessage);
    }

    [Fact]
    public async Task Validate_UsernameIsCurrentEmailAddress_ReturnsError()
    {
        // Act
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            EmailAddress = null
        };
        _mockUserRepository
            .Setup(x => x.ExistsByEmailAddressAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        // Arrange
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Username)
            .WithErrorMessage(UpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage);
    }

    [Fact]
    public async Task Validate_UsernameAlreadyExists_ReturnsError()
    {
        // Act
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid() with
        {
            Username = "new",
            EmailAddress = null
        };
        _mockUserRepository
            .Setup(x => x.ExistsByUsernameAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Arrange
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UpdateUserRequestDtoValidator.UsernameInUseErrorMessage);
    }

    [Fact]
    public async Task Validate_ValidRequest_IsValid()
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