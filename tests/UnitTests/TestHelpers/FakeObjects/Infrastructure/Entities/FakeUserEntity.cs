using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeUserEntity
{
    public static UserEntity CreateValid(IFixture fixture)
    {
        return new UserEntity
        {
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedAt = fixture.Create<DateTime>(),
            Id = fixture.Create<Guid>(),
            Username = fixture.Create<string>(),
            EmailAddress = fixture.Create<string>(),
            PasswordHash = fixture.Create<string>()
        };
    }
}