using AutoMapper;
using Core.Enums;
using Core.Models;
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

public class UserRepositoryTests : BaseTestClass
{
    private readonly UserManagementDbContext _context;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        var mapper = mapperConfig.CreateMapper();

        _context = new UserManagementDbContext(options);
        _userRepository = new UserRepository(_context, mapper);
    }

    [Fact]
    public async Task ExistsByIdAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

        // Act
        var exists = await _userRepository.ExistsByIdAsync(userEntity.UserId);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

        // Act
        var exists = await _userRepository.ExistsByUsernameAsync(userEntity.Username);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByUsernameAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

        // Act
        var exists = await _userRepository.ExistsByEmailAddressAsync(userEntity.EmailAddress);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetByEmailAddressAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

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
        (await _context.Users.FindAsync((Guid)user.UserId)).Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_GivenUser_Updates()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = false
        };

        var user = FakeUser.CreateValid(Fixture) with
        {
            UserId = new UserId(userEntity.UserId)
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        // Act
        await _userRepository.UpdateAsync(user);

        // Assert
        var updatedUser = await _context.Users.FindAsync((Guid)user.UserId);
        updatedUser.Should().NotBeNull();
        updatedUser!.UserId.Should().Be(user.UserId);
        updatedUser.Username.Should().Be(user.Username);
        updatedUser.EmailAddress.Should().Be(user.EmailAddress);
        updatedUser.PasswordSalt.Should().Be(user.PasswordSalt);
        updatedUser.PasswordHash.Should().Be(user.PasswordHash);
        updatedUser.UserRoleId.Should().Be((int)user.UserRole);
        updatedUser.UpdatedAt.Should().Be(user.UpdatedAt);
    }

    [Fact]
    public async Task GetAsync_UserIsDeleted_ReturnsFalse()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture) with
        {
            IsDeleted = true
        };

        var user = FakeUser.CreateValid(Fixture) with
        {
            UserId = new UserId(userEntity.UserId)
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _userRepository.ExistsByIdAsync(user.UserId);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task GetCountAsync_ExistingUsers_ReturnsNumberOfUsers()
    {
        // Arrange
        var pageRequest = FakePagedRequest.CreateValid(Fixture) with
        {
            PageNumber = new PageNumber(1),
            PageSize = new PageSize(10),
            SortBy = null,
            SortOrder = null,
            SearchField = null,
            SearchTerm = null
        };

        var users = Enumerable.Range(0, 20)
            .Select(_ => FakeUserEntity.CreateValid(Fixture) with
            {
                IsDeleted = false
            })
            .ToList();

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var count = await _userRepository.GetCountAsync(pageRequest);

        // Assert
        count.Should().Be(users.Count);
    }

    [Fact]
    public async Task GetAllAsync_TwoPages_ReturnsCorrectNumberOfUsers()
    {
        // Arrange
        const int numberOfExpectedPages = 2;
        var pagedRequest = FakePagedRequest.CreateValid(Fixture) with
        {
            PageNumber = new PageNumber(1),
            PageSize = new PageSize(10),
            SortBy = null,
            SearchField = null
        };

        var users = Enumerable.Range(0, numberOfExpectedPages * pagedRequest.PageSize)
            .Select(_ => FakeUserEntity.CreateValid(Fixture) with
            {
                IsDeleted = false
            })
            .ToList();

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var pagedResponse = await _userRepository.GetAllAsync<User>(pagedRequest);
        var nextPagedResponse = await _userRepository.GetAllAsync<User>(pagedRequest with
        {
            PageNumber = new PageNumber(2)
        });

        // Assert
        pagedResponse.PageNumber.Should().Be(new PageNumber(1));
        pagedResponse.PageSize.Should().Be(new PageSize(10));
        pagedResponse.PageCount.Should().Be(new PageCount(numberOfExpectedPages));
        pagedResponse.TotalCount.Should().Be(new TotalCount(users.Count));
        pagedResponse.HasMore.Should().Be(new HasMore(true));
        pagedResponse.Data.Should().HaveCount(10);

        nextPagedResponse.PageNumber.Should().Be(new PageNumber(2));
        nextPagedResponse.PageSize.Should().Be(new PageSize(10));
        nextPagedResponse.PageCount.Should().Be(new PageCount(numberOfExpectedPages));
        nextPagedResponse.TotalCount.Should().Be(new TotalCount(users.Count));
        nextPagedResponse.HasMore.Should().Be(new HasMore(false));
        nextPagedResponse.Data.Should().HaveCount(10);
    }

    [Fact]
    public async Task GetAllAsync_SortBy_ReturnsCorrectNumberOfUsers()
    {
        // Arrange
        var pagedRequest = FakePagedRequest.CreateValid(Fixture) with
        {
            PageNumber = new PageNumber(1),
            PageSize = new PageSize(10),
            SortBy = new QueryField("UserId"),
            SearchField = null
        };

        var users = Enumerable.Range(0, pagedRequest.PageSize)
            .Select(_ => FakeUserEntity.CreateValid(Fixture) with
            {
                IsDeleted = false
            })
            .ToList();

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var pagedResponse = await _userRepository.GetAllAsync<User>(pagedRequest);
        var sortedPagedResponse = await _userRepository.GetAllAsync<User>(pagedRequest with
        {
            SortOrder = SortOrder.Desc
        });

        // Assert
        pagedResponse.Data.Select(x => x.UserId).Should()
            .Equal(sortedPagedResponse.Data.Select(x => x.UserId).Reverse());
    }

    [Fact]
    public async Task GetAllAsync_SearchField_ReturnsCorrectNumberOfUsers()
    {
        // Arrange
        var users = Enumerable.Range(0, 10)
            .Select(_ => FakeUserEntity.CreateValid(Fixture) with
            {
                IsDeleted = false
            })
            .ToList();

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        var pagedRequest = FakePagedRequest.CreateValid(Fixture) with
        {
            PageNumber = new PageNumber(1),
            PageSize = new PageSize(10),
            SortBy = null,
            SearchField = new QueryField("UserId"),
            SearchTerm = new SearchTerm(users[0].UserId.ToString())
        };

        // Act
        var pagedResponse = await _userRepository.GetAllAsync<User>(pagedRequest);

        // Assert
        pagedResponse.Data.Should().HaveCount(1);
        pagedResponse.Data[0].UserId.Should().Be(new UserId(users[0].UserId));
    }
}