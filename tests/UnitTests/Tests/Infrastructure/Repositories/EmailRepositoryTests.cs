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

public class EmailRepositoryTests : BaseTestClass
{
    private readonly PennyPlannerDbContext _context;
    private readonly EmailRepository _emailRepository;

    public EmailRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<PennyPlannerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        var mapper = mapperConfig.CreateMapper();

        _context = new PennyPlannerDbContext(options);
        _emailRepository = new EmailRepository(_context, mapper);
    }

    [Fact]
    public async Task CreateAsync_GivenEmailMessage_Inserts()
    {
        // Arrange
        var emailMessage = FakeEmailMessage.CreateValid(Fixture);

        // Act
        await _emailRepository.CreateAsync(emailMessage);

        // Assert
        (await _context.Emails.FindAsync((Guid)emailMessage.EmailId)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenEmailMessage_Updates()
    {
        // Arrange
        var emailMessageEntity = FakeEmailMessageOutboxEntity.CreateValid(Fixture);
        var emailMessage = FakeEmailMessage.CreateValid(Fixture) with
        {
            EmailId = new EmailId(emailMessageEntity.EmailId)
        };

        await _context.Emails.AddAsync(emailMessageEntity);
        await _context.SaveChangesAsync();

        // Act
        await _emailRepository.UpdateAsync(emailMessage);

        // Assert
        var updatedEmail = await _context.Emails.FindAsync((Guid)emailMessage.EmailId);
        updatedEmail.Should().NotBeNull();
        updatedEmail!.EmailId.Should().Be(emailMessage.EmailId);
        updatedEmail.UpdatedAt.Should().Be(emailMessage.UpdatedAt);
        updatedEmail.UpdatedBy.Should().Be(emailMessage.UpdatedBy);
    }

    [Fact]
    public async Task GetAsync_ExistingEmail_ReturnsTrue()
    {
        // Arrange
        var emailMessageEntity = FakeEmailMessageOutboxEntity.CreateValid(Fixture);
        _context.Emails.Add(emailMessageEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _emailRepository.GetAsync(emailMessageEntity.EmailId);

        // Assert
        exists.Should().NotBeNull();
    }

    [Fact]
    public async Task ExistsAsync_NonExistingEmailMessage_ReturnsFalse()
    {
        // Arrange
        var emailMessageEntity = FakeEmailMessageOutboxEntity.CreateValid(Fixture);

        // Act
        var exists = await _emailRepository.ExistsAsync(emailMessageEntity.EmailId);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetAsync_ExistingEmailMessage_ReturnsEmailMessage()
    {
        // Arrange
        var emailMessageEntity = FakeEmailMessageOutboxEntity.CreateValid(Fixture);
        _context.Emails.Add(emailMessageEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _emailRepository.GetAsync(emailMessageEntity.EmailId);

        // Assert
        exists.Should().NotBeNull();
    }
}