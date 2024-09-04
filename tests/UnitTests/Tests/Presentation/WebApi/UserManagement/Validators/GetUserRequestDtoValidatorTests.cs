using Core.Enums;
using Core.Interfaces.Repositories;
using FluentValidation.TestHelper;
using Moq;
using Presentation.WebApi.UserManagement.Validators;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.UserManagement.Models.Requests;

namespace UnitTests.Tests.Presentation.WebApi.UserManagement.Validators;

public class GetUserRequestDtoValidatorTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly GetUserRequestDtoValidator _validator;

    public GetUserRequestDtoValidatorTests()
    {
        _validator = new GetUserRequestDtoValidator(_mockUserRepository.Object);
    }

    [Fact]
    public async Task ValidateAsync_UserIdIsEmpty_ReturnsError()
    {
        // Arrange
        var getUserRequestDto = FakeUserRequestDto.CreateValid(Fixture) with
        {
            UserId = Guid.Empty
        };

        // Act
        var result = await _validator.TestValidateAsync(getUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorCode("NotEmptyValidator");
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        var getUserRequestDto = FakeUserRequestDto.CreateValid(Fixture);
        _mockUserRepository
            .Setup(x => x.ExistsByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var result = await _validator.TestValidateAsync(getUserRequestDto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage(GetUserRequestDtoValidator.UserDoesNotExistErrorMessage);
    }

    [Fact]
    public async Task ValidateAsync_ValidRequest_IsValid()
    {
        // Arrange
        var getUserRequestDto = FakeUserRequestDto.CreateValid(Fixture);
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
        var result = await _validator.TestValidateAsync(getUserRequestDto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}