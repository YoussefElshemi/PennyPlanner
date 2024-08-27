using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeLoginEntity
{
    public static LoginEntity CreateValid(IFixture fixture)
    {
        var userEntity = FakeUserEntity.CreateValid(fixture);

        return new LoginEntity
        {
            LoginId = fixture.Create<Guid>(),
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            IpAddress = fixture.Create<string>(),
            RefreshToken = fixture.Create<string>(),
            ExpiresAt = fixture.Create<DateTime>(),
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>(),
            IsRevoked = false,
            RevokedBy = fixture.Create<string>(),
            RevokedAt = fixture.Create<DateTime>()
        };
    }
}