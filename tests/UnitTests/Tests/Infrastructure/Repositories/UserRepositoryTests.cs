using FluentAssertions;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Repositories;

public class UserRepositoryTests : BaseTestClass
{
    private readonly PennyPlannerDbContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PennyPlannerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new PennyPlannerDbContext(options);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task ExistsByIdAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.ExistsByIdAsync(userEntity.Id);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByIdAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByIdAsync(userEntity.Id);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserByIdAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetUserByIdAsync(userEntity.Id);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByIdAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.GetUserByIdAsync(userEntity.Id);

        // Assert
        exists.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.ExistsByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByUsernameAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetUserByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserByUsernameAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.GetUserByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeNull();
    }
}