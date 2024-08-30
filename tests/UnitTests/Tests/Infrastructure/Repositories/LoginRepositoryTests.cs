using AutoMapper;
using Core.ValueObjects;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Mappers;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Repositories;

public class LoginRepositoryTests : BaseTestClass
{
    private readonly PennyPlannerDbContext _context;
    private readonly LoginRepository _loginRepository;

    public LoginRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PennyPlannerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        var mapper = mapperConfig.CreateMapper();

        _context = new PennyPlannerDbContext(options);
        _loginRepository = new LoginRepository(_context, mapper);
    }

    [Fact]
    public async Task CreateAsync_GivenLogin_Inserts()
    {
        // Arrange
        var login = FakeLogin.CreateValid(Fixture);

        // Act
        await _loginRepository.CreateAsync(login);

        // Assert
        (await _context.Logins.FindAsync((Guid)login.LoginId)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenLogin_Updates()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);
        var login = FakeLogin.CreateValid(Fixture) with
        {
            LoginId = new LoginId(loginEntity.LoginId),
            IsRevoked = new IsRevoked(true)
        };

        await _context.Logins.AddAsync(loginEntity);
        await _context.SaveChangesAsync();

        // Act
        await _loginRepository.UpdateAsync(login);

        // Assert
        var updatedLogin = await _context.Logins.FindAsync((Guid)login.LoginId);
        updatedLogin.Should().NotBeNull();
        updatedLogin!.LoginId.Should().Be(login.LoginId);
        updatedLogin.IsRevoked.Should().Be(login.IsRevoked);
        updatedLogin.UpdatedAt.Should().Be(login.UpdatedAt);
        updatedLogin.UpdatedBy.Should().Be(login.UpdatedBy);
    }

    [Fact]
    public async Task GetAsync_ExistingLogin_ReturnsTrue()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);
        _context.Logins.Add(loginEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _loginRepository.GetAsync(loginEntity.RefreshToken);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task ExistsAsync_NonExistingUser_ReturnsFalse()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);

        // Act
        var exists = await _loginRepository.ExistsAsync(loginEntity.RefreshToken);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);
        _context.Logins.Add(loginEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _loginRepository.GetAsync(loginEntity.RefreshToken);

        // Assert
        exists.Should().NotBeNull();
    }
}