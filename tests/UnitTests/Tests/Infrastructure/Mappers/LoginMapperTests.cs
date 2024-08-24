using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Mappers;

public class LoginMapperTests : BaseTestClass
{
    [Fact]
    public void MapFromEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);

        // Act
        var login = LoginMapper.MapFromEntity(loginEntity);

        // Assert
        login.LoginId.Should().Be(new LoginId(loginEntity.LoginId));
        login.UserId.Should().Be(new UserId(loginEntity.UserId));
        login.User.Should().Be(UserMapper.MapFromEntity(loginEntity.UserEntity));
        login.IpAddress.Should().Be(new IpAddress(loginEntity.IpAddress));
        login.RefreshToken.Should().Be(new RefreshToken(loginEntity.RefreshToken));
        login.ExpiresAt.Should().Be(new ExpiresAt(loginEntity.ExpiresAt));
        login.IsRevoked.Should().Be(new IsRevoked(loginEntity.IsRevoked));
        login.RevokedAt.Should().Be(new RevokedAt(loginEntity.RevokedAt!.Value));
        login.CreatedAt.Should().Be(new CreatedAt(loginEntity.CreatedAt));
        login.UpdatedAt.Should().Be(new UpdatedAt(loginEntity.UpdatedAt));
    }

    [Fact]
    public void MapToEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var login = FakeLogin.CreateValid(Fixture);

        // Act
        var loginEntity = LoginMapper.MapToEntity(login);

        // Assert
        loginEntity.LoginId.Should().Be(login.LoginId.ToString());
        loginEntity.UserId.Should().Be(login.UserId.ToString());
        loginEntity.IpAddress.Should().Be(login.IpAddress.ToString());
        loginEntity.RefreshToken.Should().Be(login.RefreshToken.ToString());
        loginEntity.ExpiresAt.Should().Be(login.ExpiresAt);
        loginEntity.IsRevoked.Should().Be(login.IsRevoked);
        loginEntity.RevokedAt.Should().Be(login.RevokedAt);
        loginEntity.CreatedAt.Should().Be(login.CreatedAt);
        loginEntity.UpdatedAt.Should().Be(login.UpdatedAt);
    }
}