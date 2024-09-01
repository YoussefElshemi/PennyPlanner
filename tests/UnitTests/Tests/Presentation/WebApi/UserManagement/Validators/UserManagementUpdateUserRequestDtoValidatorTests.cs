using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.UserManagement.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.UserManagement.Validators;

public class UserManagementUpdateUserRequestDtoValidatorTests : BaseTestClass
{
    private readonly User _authenticatedUser;
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly UserManagementUpdateUserRequestDtoValidator _validator;

    public UserManagementUpdateUserRequestDtoValidatorTests()
    {
        _authenticatedUser = FakeUser.CreateValid(Fixture);
        _validator = new UserManagementUpdateUserRequestDtoValidator(_mockUserRepository.Object, _authenticatedUser);
    }

    [Fact]
    public async Task ValidateAsync_UserIdIsEmpty_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture);
        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(UserManagementUpdateUserRequestDtoValidator.UserDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UserIsAdmin_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture) with
        {
            UserRole = UserRole.Admin
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(UserManagementUpdateUserRequestDtoValidator.CanNotUpdateAdminErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UserRoleIsInvalid_ReturnsError()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = _authenticatedUser.UserId,
            UserRole = "invalid"
        };

        var user = FakeUser.CreateValid(Fixture);

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserRole)
            .WithErrorMessage(UserManagementUpdateUserRequestDtoValidator.InvalidUserRoleErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UserRoleDidNotChange_ReturnsError()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = _authenticatedUser.UserId,
            UserRole = user.UserRole.ToString()
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UserManagementUpdateUserRequestDtoValidator.FieldDidNotUpdateErrorMessage(nameof(UserRole)));
    }

    [Fact]
    public async Task ValidateAsync_CanNotUpdateAdminUserRole_ReturnsError()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture) with
        {
            UserRole = UserRole.Admin
        };
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = _authenticatedUser.UserId,
            UserRole = UserRole.User.ToString()
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(UserManagementUpdateUserRequestDtoValidator.CanNotUpdateAdminErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UpdateUserToAdmin_IsValid()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture) with
        {
            UserRole = UserRole.User
        };
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = _authenticatedUser.UserId,
            UserRole = UserRole.Admin.ToString()
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidateAsync_UserIsAdminButUpdatingAlsoAuthenticatedUser_IsValid()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture) with
        {
            UserId = _authenticatedUser.UserId
        };

        var user = FakeUser.CreateValid(Fixture) with
        {
            UserRole = UserRole.Admin
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var updateUserRequestDto = FakeUpdateUserRequestDto.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture) with
        {
            UserRole = UserRole.User
        };

        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(user);

        // Act
        var result = await _validator.TestValidateAsync(updateUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}