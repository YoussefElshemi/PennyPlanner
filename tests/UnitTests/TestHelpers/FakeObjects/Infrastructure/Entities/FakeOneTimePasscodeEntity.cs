using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeOneTimePasscodeEntity
{
    public static OneTimePasscodeEntity CreateValid(IFixture fixture)
    {
        return new OneTimePasscodeEntity
        {
            OneTimePasscodeId = fixture.Create<Guid>(),
            UserId = fixture.Create<Guid>(),
            UserEntity = FakeUserEntity.CreateValid(fixture),
            IpAddress = fixture.Create<string>(),
            Passcode = fixture.Create<string>(),
            IsUsed = fixture.Create<bool>(),
            ExpiresAt = fixture.Create<DateTime>(),
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}