using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakePasswordResetEntity
{
    public static PasswordResetEntity CreateValid(IFixture fixture)
    {
        var userEntity = FakeUserEntity.CreateValid(fixture);

        return new PasswordResetEntity
        {
            PasswordResetId = fixture.Create<Guid>(),
            UserId = userEntity.UserId,
            UserEntity = userEntity,
            ResetToken = fixture.Create<string>(),
            IsUsed = false,
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}