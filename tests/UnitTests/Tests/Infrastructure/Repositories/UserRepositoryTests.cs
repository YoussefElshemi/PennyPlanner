using Core.ValueObjects;
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
        var exists = await _userRepository.ExistsByIdAsync(userEntity.UserId);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsByIdAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByIdAsync(userEntity.UserId);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.GetByIdAsync(userEntity.UserId);

        // Assert
        exists.Should().NotBeNull();
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
    public async Task ExistsByUsernameAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByUsernameAsync_ExistingUser_ReturnsUser()
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
    public async Task ExistsByEmailAddressAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var exists = await _userRepository.ExistsByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByEmailAddressAsync_ExistingUser_ReturnsUser()
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
    public async Task CreateAsync_GivenUser_Inserts()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        await _userRepository.CreateAsync(user);

        // Assert
        (await _context.Users.FindAsync(user.UserId.Value)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenUser_Updates()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);
        var user = FakeUser.CreateValid(Fixture) with
        {
            UserId = new UserId(userEntity.UserId)
        };
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        // Act
        await _userRepository.UpdateAsync(user);

        // Assert
        var updatedUser = await _context.Users.FindAsync(user.UserId.Value);
        updatedUser.Should().NotBeNull();
        updatedUser!.UserId.Should().Be(user.UserId.Value);
        updatedUser.EmailAddress.Should().Be(user.EmailAddress);
        updatedUser.PasswordSalt.Should().Be(user.PasswordSalt);
        updatedUser.PasswordHash.Should().Be(user.PasswordHash);
        updatedUser.UserRoleId.Should().Be((int)user.UserRole);
        updatedUser.UpdatedAt.Should().Be(user.UpdatedAt);
    }
}