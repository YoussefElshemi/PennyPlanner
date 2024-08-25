using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.Services;
using FluentAssertions;
using Moq;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Core.Services;

public class UserServiceTests : BaseTestClass
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockUserRepository.Object,
            MockTimeProvider.Object);
    }

    [Fact]
    public async Task CreateUser_SuccessfulCreation_ReturnsUser()
    {
        // Arrange
        var createUserRequest = FakeCreateUserRequest.CreateValid(Fixture);
        _mockUserRepository
            .Setup(x => x.CreateAsync(It.IsAny<User>()))
            .Verifiable();

        // Act
        var user = await _userService.CreateAsync(createUserRequest);

        // Assert
        user.Username.Should().Be(createUserRequest.Username);
        user.EmailAddress.Should().Be(createUserRequest.EmailAddress);
        user.PasswordHash.Should().NotBeNull();
        user.PasswordSalt.Should().NotBeNull();
        user.UserRole.Should().Be(UserRole.User);
    }
}