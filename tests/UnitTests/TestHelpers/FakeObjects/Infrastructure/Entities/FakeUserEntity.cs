using AutoFixture;
using Core.Enums;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeUserEntity
{
    public static UserEntity CreateValid(IFixture fixture)
    {
        return new UserEntity
        {
            Id = fixture.Create<Guid>(),
            Username = fixture.Create<string>(),
            EmailAddress = fixture.Create<string>(),
            PasswordHash = fixture.Create<string>(),
            UserRole = fixture.Create<UserRole>().ToString(),
            CreatedBy = fixture.Create<Guid>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}