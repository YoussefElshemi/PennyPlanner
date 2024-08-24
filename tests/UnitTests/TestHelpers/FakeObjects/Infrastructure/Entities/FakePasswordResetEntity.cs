using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakePasswordResetEntity
{
    public static PasswordResetEntity CreateValid(IFixture fixture)
    {
        return new PasswordResetEntity
        {
            PasswordResetId = fixture.Create<Guid>(),
            UserId = fixture.Create<Guid>(),
            UserEntity = FakeUserEntity.CreateValid(fixture),
            ResetToken = fixture.Create<string>(),
            IsUsed = fixture.Create<bool>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}