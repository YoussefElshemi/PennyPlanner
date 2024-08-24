using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeLoginEntity
{
    public static LoginEntity CreateValid(IFixture fixture)
    {
        return new LoginEntity
        {
            LoginId = fixture.Create<Guid>(),
            UserId = fixture.Create<Guid>(),
            UserEntity = FakeUserEntity.CreateValid(fixture),
            IpAddress = fixture.Create<string>(),
            RefreshToken = fixture.Create<string>(),
            ExpiresAt = fixture.Create<DateTime>(),
            IsRevoked = fixture.Create<bool>(),
            RevokedAt = fixture.Create<DateTime>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}