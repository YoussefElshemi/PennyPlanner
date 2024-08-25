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

public class PasswordResetRepositoryTests : BaseTestClass
{
    private readonly PennyPlannerDbContext _context;
    private readonly PasswordResetRepository _passwordResetRepository;

    public PasswordResetRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PennyPlannerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        var mapper = mapperConfig.CreateMapper();

        _context = new PennyPlannerDbContext(options);
        _passwordResetRepository = new PasswordResetRepository(_context, mapper);
    }

    [Fact]
    public async Task CreateAsync_GivenPasswordReset_Inserts()
    {
        // Arrange
        var passwordReset = FakePasswordReset.CreateValid(Fixture);

        // Act
        await _passwordResetRepository.CreateAsync(passwordReset);

        // Assert
        (await _context.PasswordResets.FindAsync(passwordReset.PasswordResetId.Value)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenPasswordReset_Updates()
    {
        // Arrange
        var passwordResetEntity = FakePasswordResetEntity.CreateValid(Fixture);
        var passwordReset = FakePasswordReset.CreateValid(Fixture) with
        {
            PasswordResetId = new PasswordResetId(passwordResetEntity.PasswordResetId)
        };
        await _context.PasswordResets.AddAsync(passwordResetEntity);
        await _context.SaveChangesAsync();

        // Act
        await _passwordResetRepository.UpdateAsync(passwordReset);

        // Assert
        var updatedPasswordReset = await _context.PasswordResets.FindAsync(passwordReset.PasswordResetId.Value);
        updatedPasswordReset.Should().NotBeNull();
        updatedPasswordReset!.PasswordResetId.Should().Be(passwordReset.PasswordResetId.Value);
        updatedPasswordReset.IsUsed.Should().Be(passwordReset.IsUsed);
        updatedPasswordReset.UpdatedAt.Should().Be(passwordReset.UpdatedAt);
    }

    [Fact]
    public async Task GetAsync_ExistingPasswordReset_ReturnsTrue()
    {
        // Arrange
        var passwordResetEntity = FakePasswordResetEntity.CreateValid(Fixture);
        _context.PasswordResets.Add(passwordResetEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _passwordResetRepository.GetAsync(passwordResetEntity.ResetToken);

        // Assert
        exists.Should().NotBeNull();
    }
}