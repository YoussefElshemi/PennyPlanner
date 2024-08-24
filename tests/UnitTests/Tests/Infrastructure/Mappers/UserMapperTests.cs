using Core.Enums;
using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Mappers;

public class UserMapperTests : BaseTestClass
{
    [Fact]
    public void MapFromEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var user = UserMapper.MapFromEntity(userEntity);

        // Assert
        user.UserId.Should().Be(new UserId(userEntity.UserId));
        user.Username.Should().Be(new Username(userEntity.Username));
        user.EmailAddress.Should().Be(new EmailAddress(userEntity.EmailAddress));
        user.PasswordHash.Should().Be(new PasswordHash(userEntity.PasswordHash));
        user.PasswordSalt.Should().Be(new PasswordSalt(userEntity.PasswordSalt));
        user.UserRole.Should().Be((UserRole)userEntity.UserRoleId);
        user.CreatedAt.Should().Be(new CreatedAt(userEntity.CreatedAt));
        user.UpdatedAt.Should().Be(new UpdatedAt(userEntity.UpdatedAt));
    }

    [Fact]
    public void MapToEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        var userEntity = UserMapper.MapToEntity(user);

        // Assert
        userEntity.UserId.Should().Be(user.UserId.ToString());
        userEntity.Username.Should().Be(user.Username.ToString());
        userEntity.EmailAddress.Should().Be(user.EmailAddress.ToString());
        userEntity.PasswordHash.Should().Be(user.PasswordHash.ToString());
        userEntity.PasswordSalt.Should().Be(user.PasswordSalt.ToString());
        userEntity.UserRoleId.Should().Be((int)user.UserRole);
        userEntity.CreatedAt.Should().Be(user.CreatedAt);
        userEntity.UpdatedAt.Should().Be(user.UpdatedAt);
    }
}