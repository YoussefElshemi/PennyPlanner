using AutoMapper;
using Core.Enums;
using Core.Models;
using Core.ValueObjects;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;
using UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

namespace UnitTests.Tests.Infrastructure.Mappers;

public class InfrastructureProfileTests : BaseTestClass
{
    private readonly IMapper _mapper;

    public InfrastructureProfileTests()
    {
        var mapperConfig = new MapperConfiguration(x => x.AddProfile<InfrastructureProfile>());
        _mapper = mapperConfig.CreateMapper();
    }

    [Fact]
    public void MapFromLoginEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var loginEntity = FakeLoginEntity.CreateValid(Fixture);

        // Act
        var login = _mapper.Map<Login>(loginEntity);

        // Assert
        login.LoginId.Should().Be(new LoginId(loginEntity.LoginId));
        login.UserId.Should().Be(new UserId(loginEntity.UserId));
        login.User.Should().Be(_mapper.Map<User>(loginEntity.UserEntity));
        login.IpAddress.Should().Be(new IpAddress(loginEntity.IpAddress));
        login.RefreshToken.Should().Be(new RefreshToken(loginEntity.RefreshToken));
        login.ExpiresAt.Should().Be(new ExpiresAt(loginEntity.ExpiresAt));
        login.CreatedBy.Should().Be(new Username(loginEntity.CreatedBy));
        login.CreatedAt.Should().Be(new CreatedAt(loginEntity.CreatedAt));
        login.UpdatedBy.Should().Be(new Username(loginEntity.UpdatedBy));
        login.UpdatedAt.Should().Be(new UpdatedAt(loginEntity.UpdatedAt));
        login.IsRevoked.Should().Be(new IsRevoked(loginEntity.IsRevoked));
        login.RevokedBy.Should().Be(new Username(loginEntity.RevokedBy!));
        login.RevokedAt.Should().Be(new RevokedAt(loginEntity.RevokedAt!.Value));
    }

    [Fact]
    public void MapToLoginEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var login = FakeLogin.CreateValid(Fixture);

        // Act
        var loginEntity = _mapper.Map<LoginEntity>(login);

        // Assert
        loginEntity.LoginId.Should().Be(login.LoginId.ToString());
        loginEntity.UserId.Should().Be(login.UserId.ToString());
        loginEntity.IpAddress.Should().Be(login.IpAddress.ToString());
        loginEntity.RefreshToken.Should().Be(login.RefreshToken.ToString());
        loginEntity.ExpiresAt.Should().Be(login.ExpiresAt);
        loginEntity.CreatedBy.Should().Be(login.CreatedBy);
        loginEntity.CreatedAt.Should().Be(login.CreatedAt);
        loginEntity.UpdatedBy.Should().Be(login.UpdatedBy);
        loginEntity.UpdatedAt.Should().Be(login.UpdatedAt);
        loginEntity.IsRevoked.Should().Be(login.IsRevoked);
        loginEntity.RevokedBy.Should().Be(loginEntity.RevokedBy);
        loginEntity.RevokedAt.Should().Be(login.RevokedAt);
    }

    [Fact]
    public void MapFromPasswordResetEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var passwordResetEntity = FakePasswordResetEntity.CreateValid(Fixture);

        // Act
        var passwordReset = _mapper.Map<PasswordReset>(passwordResetEntity);

        // Assert
        passwordReset.PasswordResetId.Should().Be(new PasswordResetId(passwordResetEntity.PasswordResetId));
        passwordReset.UserId.Should().Be(new UserId(passwordResetEntity.UserId));
        passwordReset.User.Should().Be(_mapper.Map<User>(passwordResetEntity.UserEntity));
        passwordReset.ResetToken.Should().Be(new PasswordResetToken(passwordResetEntity.ResetToken));
        passwordReset.IsUsed.Should().Be(new IsUsed(passwordResetEntity.IsUsed));
        passwordReset.CreatedBy.Should().Be(new Username(passwordResetEntity.CreatedBy));
        passwordReset.CreatedAt.Should().Be(new CreatedAt(passwordResetEntity.CreatedAt));
        passwordReset.UpdatedBy.Should().Be(new Username(passwordResetEntity.UpdatedBy));
        passwordReset.UpdatedAt.Should().Be(new UpdatedAt(passwordResetEntity.UpdatedAt));
    }

    [Fact]
    public void MapToPasswordResetEntityEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var passwordReset = FakePasswordReset.CreateValid(Fixture);

        // Act
        var passwordResetEntity = _mapper.Map<PasswordResetEntity>(passwordReset);

        // Assert
        passwordResetEntity.PasswordResetId.Should().Be(passwordReset.PasswordResetId.ToString());
        passwordResetEntity.UserId.Should().Be(passwordReset.UserId.ToString());
        passwordResetEntity.ResetToken.Should().Be(passwordReset.ResetToken.ToString());
        passwordResetEntity.IsUsed.Should().Be(passwordReset.IsUsed);
        passwordResetEntity.CreatedBy.Should().Be(passwordReset.CreatedBy);
        passwordResetEntity.CreatedAt.Should().Be(passwordReset.CreatedAt);
        passwordResetEntity.UpdatedBy.Should().Be(passwordReset.UpdatedBy);
        passwordResetEntity.UpdatedAt.Should().Be(passwordReset.UpdatedAt);
    }

    [Fact]
    public void MapFromUserEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var userEntity = FakeUserEntity.CreateValid(Fixture);

        // Act
        var user = _mapper.Map<User>(userEntity);

        // Assert
        user.UserId.Should().Be(new UserId(userEntity.UserId));
        user.Username.Should().Be(new Username(userEntity.Username));
        user.EmailAddress.Should().Be(new EmailAddress(userEntity.EmailAddress));
        user.PasswordHash.Should().Be(new PasswordHash(userEntity.PasswordHash));
        user.PasswordSalt.Should().Be(new PasswordSalt(userEntity.PasswordSalt));
        user.UserRole.Should().Be((UserRole)userEntity.UserRoleId);
        user.IsDeleted.Should().Be(new IsDeleted(userEntity.IsDeleted));
        user.DeletedBy.Should().Be(new Username(userEntity.DeletedBy!));
        user.DeletedAt.Should().Be(new DeletedAt(userEntity.DeletedAt!.Value));
        user.CreatedBy.Should().Be(new Username(userEntity.CreatedBy));
        user.CreatedAt.Should().Be(new CreatedAt(userEntity.CreatedAt));
        user.UpdatedBy.Should().Be(new Username(userEntity.UpdatedBy));
        user.UpdatedAt.Should().Be(new UpdatedAt(userEntity.UpdatedAt));
    }

    [Fact]
    public void MapToUserEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var user = FakeUser.CreateValid(Fixture);

        // Act
        var userEntity = _mapper.Map<UserEntity>(user);

        // Assert
        userEntity.UserId.Should().Be(user.UserId.ToString());
        userEntity.Username.Should().Be(user.Username.ToString());
        userEntity.EmailAddress.Should().Be(user.EmailAddress.ToString());
        userEntity.PasswordHash.Should().Be(user.PasswordHash.ToString());
        userEntity.PasswordSalt.Should().Be(user.PasswordSalt.ToString());
        userEntity.UserRoleId.Should().Be((int)user.UserRole);
        userEntity.CreatedBy.Should().Be(user.CreatedBy);
        userEntity.UpdatedAt.Should().Be(user.UpdatedAt);
        userEntity.UpdatedBy.Should().Be(user.UpdatedBy);
        userEntity.UpdatedAt.Should().Be(user.UpdatedAt);
    }

    [Fact]
    public void MapFromOneTimePasscodeEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var oneTimePasscodeEntity = FakeOneTimePasscodeEntity.CreateValid(Fixture);

        // Act
        var oneTimePasscode = _mapper.Map<OneTimePasscode>(oneTimePasscodeEntity);

        // Assert
        oneTimePasscode.OneTimePasscodeId.Should().Be(new OneTimePasscodeId(oneTimePasscodeEntity.OneTimePasscodeId));
        oneTimePasscode.UserId.Should().Be(new UserId(oneTimePasscodeEntity.UserId));
        oneTimePasscode.IpAddress.Should().Be(new IpAddress(oneTimePasscodeEntity.IpAddress));
        oneTimePasscode.Passcode.Should().Be(new Passcode(oneTimePasscodeEntity.Passcode));
        oneTimePasscode.IsUsed.Should().Be(new IsUsed(oneTimePasscodeEntity.IsUsed));
        oneTimePasscode.ExpiresAt.Should().Be(new ExpiresAt(oneTimePasscodeEntity.ExpiresAt));
        oneTimePasscode.CreatedBy.Should().Be(new Username(oneTimePasscodeEntity.CreatedBy));
        oneTimePasscode.CreatedAt.Should().Be(new CreatedAt(oneTimePasscodeEntity.CreatedAt));
        oneTimePasscode.UpdatedBy.Should().Be(new Username(oneTimePasscodeEntity.UpdatedBy));
        oneTimePasscode.UpdatedAt.Should().Be(new UpdatedAt(oneTimePasscodeEntity.UpdatedAt));
    }

    [Fact]
    public void MapToOneTimePasscodeEntity_GivenEntity_ReturnsModel()
    {
        // Arrange
        var oneTimePasscode = FakeOneTimePasscode.CreateValid(Fixture);

        // Act
        var oneTimePasscodeEntity = _mapper.Map<OneTimePasscodeEntity>(oneTimePasscode);

        // Assert
        oneTimePasscodeEntity.OneTimePasscodeId.Should().Be(oneTimePasscode.OneTimePasscodeId.ToString());
        oneTimePasscodeEntity.UserId.Should().Be(oneTimePasscode.UserId);
        oneTimePasscodeEntity.IpAddress.Should().Be(oneTimePasscode.IpAddress);
        oneTimePasscodeEntity.Passcode.Should().Be(oneTimePasscode.Passcode);
        oneTimePasscodeEntity.IsUsed.Should().Be(oneTimePasscode.IsUsed);
        oneTimePasscodeEntity.ExpiresAt.Should().Be(oneTimePasscode.ExpiresAt);
        oneTimePasscodeEntity.CreatedBy.Should().Be(oneTimePasscode.CreatedBy);
        oneTimePasscodeEntity.UpdatedAt.Should().Be(oneTimePasscode.UpdatedAt);
        oneTimePasscodeEntity.UpdatedBy.Should().Be(oneTimePasscode.UpdatedBy);
        oneTimePasscodeEntity.UpdatedAt.Should().Be(oneTimePasscode.UpdatedAt);
    }
}