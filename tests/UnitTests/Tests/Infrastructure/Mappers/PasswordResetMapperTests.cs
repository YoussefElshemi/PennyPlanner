using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Mappers;

public class PasswordResetMapperTests : BaseTestClass
{
    [Fact]
    public void MapFromEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var passwordResetEntity = FakePasswordResetEntity.CreateValid(Fixture);

        // Act
        var passwordReset = PasswordResetMapper.MapFromEntity(passwordResetEntity);

        // Assert
        passwordReset.PasswordResetId.Should().Be(new PasswordResetId(passwordResetEntity.PasswordResetId));
        passwordReset.UserId.Should().Be(new UserId(passwordResetEntity.UserId));
        passwordReset.User.Should().Be(UserMapper.MapFromEntity(passwordResetEntity.UserEntity));
        passwordReset.ResetToken.Should().Be(new PasswordResetToken(passwordResetEntity.ResetToken));
        passwordReset.IsUsed.Should().Be(new IsUsed(passwordResetEntity.IsUsed));
        passwordReset.CreatedAt.Should().Be(new CreatedAt(passwordResetEntity.CreatedAt));
        passwordReset.UpdatedAt.Should().Be(new UpdatedAt(passwordResetEntity.UpdatedAt));
    }

    [Fact]
    public void MapToEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var passwordReset = FakePasswordReset.CreateValid(Fixture);

        // Act
        var passwordResetEntity = PasswordResetMapper.MapToEntity(passwordReset);

        // Assert
        passwordResetEntity.PasswordResetId.Should().Be(passwordReset.PasswordResetId.ToString());
        passwordResetEntity.UserId.Should().Be(passwordReset.UserId.ToString());
        passwordResetEntity.ResetToken.Should().Be(passwordReset.ResetToken.ToString());
        passwordResetEntity.IsUsed.Should().Be(passwordReset.IsUsed);
        passwordResetEntity.CreatedAt.Should().Be(passwordReset.CreatedAt);
        passwordResetEntity.UpdatedAt.Should().Be(passwordReset.UpdatedAt);
    }
}