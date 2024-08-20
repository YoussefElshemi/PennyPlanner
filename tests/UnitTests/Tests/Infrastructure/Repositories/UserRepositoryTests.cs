using FluentAssertions;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
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
    public async Task GetByIdAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetByIdAsync(userEntity.Id);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.GetByIdAsync(userEntity.Id);

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
    public async Task GetByUsernameAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByUsernameAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.GetByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeNull();
    }

    [Fact]
    public async Task ExistsByEmailAddressAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.ExistsByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByEmailAddressAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByEmailAddressAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByEmailAddressAsync_NonExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.GetByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_GivenUser_ReturnsVoid()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        await _userRepository.CreateAsync(user);

        // Assert
        (await _context.Users.FindAsync(user.UserId.Value)).Should().NotBeNull();
    }
}