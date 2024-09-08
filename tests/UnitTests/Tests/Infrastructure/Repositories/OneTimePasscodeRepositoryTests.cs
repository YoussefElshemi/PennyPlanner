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

public class OneTimePasscodeRepositoryTests : BaseTestClass
{
    private readonly UserManagementDbContext _context;
    private readonly OneTimePasscodeRepository _oneTimePasscodeRepository;

    public OneTimePasscodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        var mapper = mapperConfig.CreateMapper();

        _context = new UserManagementDbContext(options);
        _oneTimePasscodeRepository = new OneTimePasscodeRepository(_context, mapper);
    }

    [Fact]
    public async Task CreateAsync_GivenOneTimePasscode_Inserts()
    {
        // Arrange
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture);

        // Act
        await _oneTimePasscodeRepository.CreateAsync(oneTimePasscode);

        // Assert
        (await _context.OneTimePasscodes.FindAsync((Guid)oneTimePasscode.OneTimePasscodeId)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenOneTimePasscode_Updates()
    {
        // Arrange
        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(Fixture);
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture) with
        {
            OneTimePasscodeId = new OneTimePasscodeId(oneTimePasscodeEntity.OneTimePasscodeId)
        };

        await _context.OneTimePasscodes.AddAsync(oneTimePasscodeEntity);
        await _context.SaveChangesAsync();

        // Act
        await _oneTimePasscodeRepository.UpdateAsync(oneTimePasscode);

        // Assert
        var updatedOneTimePasscode = await _context.OneTimePasscodes.FindAsync((Guid)oneTimePasscode.OneTimePasscodeId);
        updatedOneTimePasscode.Should().NotBeNull();
        updatedOneTimePasscode!.OneTimePasscodeId.Should().Be(oneTimePasscode.OneTimePasscodeId);
        updatedOneTimePasscode.UpdatedAt.Should().Be(oneTimePasscode.UpdatedAt);
        updatedOneTimePasscode.UpdatedBy.Should().Be(oneTimePasscode.UpdatedBy);
    }

    [Fact]
    public async Task GetAsync_ExistingOneTimePasscode_ReturnsTrue()
    {
        // Arrange
        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(Fixture);
        _context.OneTimePasscodes.Add(oneTimePasscodeEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _oneTimePasscodeRepository.GetAsync(oneTimePasscodeEntity.UserId, oneTimePasscodeEntity.Passcode);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task ExistsAsync_NonExistingOneTimePasscode_ReturnsFalse()
    {
        // Arrange
        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(Fixture);

        // Act
        var exists = await _oneTimePasscodeRepository.ExistsAsync(oneTimePasscodeEntity.UserId, oneTimePasscodeEntity.IpAddress);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_ExistingOneTimePasscode_ReturnsOneTimePasscode()
    {
        // Arrange
        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(Fixture);
        _context.OneTimePasscodes.Add(oneTimePasscodeEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _oneTimePasscodeRepository.GetAsync(oneTimePasscodeEntity.UserId, oneTimePasscodeEntity.Passcode);

        // Assert
        exists.Should().NotBeNull();
    }
}