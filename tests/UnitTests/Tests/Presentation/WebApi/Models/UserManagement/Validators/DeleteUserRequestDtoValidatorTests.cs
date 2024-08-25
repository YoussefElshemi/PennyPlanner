using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.Models.UserManagement.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.UserManagement;

namespace UnitTests.Tests.Presentation.WebApi.Models.UserManagement.Validators;

public class DeleteUserRequestDtoValidatorTests : BaseTestClass
{
    private readonly User _authenticatedUser;
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly DeleteUserRequestDtoValidator _validator;

    public DeleteUserRequestDtoValidatorTests()
    {
        _authenticatedUser = FakeUser.CreateValid(Fixture);
        _validator = new DeleteUserRequestDtoValidator(_mockUserRepository.Object, _authenticatedUser);
    }

    [Fact]
    public async Task ValidateAsync_UserIdIsEmpty_ReturnsError()
    {
        // Arrange
        var deleteUserRequestDto = FakeGetUserRequestDto.CreateValid(Fixture) with
        {
            UserId = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(deleteUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        var deleteUserRequestDto = FakeGetUserRequestDto.CreateValid(Fixture);
        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(deleteUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(DeleteUserRequestDtoValidator.UserDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UserIsAdmin_ReturnsError()
    {
        // Arrange
        var deleteUserRequestDto = FakeGetUserRequestDto.CreateValid(Fixture);
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
        var result = await _validator.TestValidateAsync(deleteUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(DeleteUserRequestDtoValidator.CanNotDeleteAdminErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_UserIsAuthenticatedUser_ReturnsError()
    {
        // Arrange
        var deleteUserRequestDto = FakeGetUserRequestDto.CreateValid(Fixture) with
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
        var result = await _validator.TestValidateAsync(deleteUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(DeleteUserRequestDtoValidator.CanNotDeleteSelfErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var deleteUserRequestDto = FakeGetUserRequestDto.CreateValid(Fixture);
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
        var result = await _validator.TestValidateAsync(deleteUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}