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
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>(),
            IsRevoked = fixture.Create<bool>(),
            RevokedBy = fixture.Create<string>(),
            RevokedAt = fixture.Create<DateTime>()
        };
    }
}