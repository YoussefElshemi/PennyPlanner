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
            UserId = fixture.Create<Guid>(),
            Username = fixture.Create<string>(),
            EmailAddress = fixture.Create<string>(),
            PasswordHash = fixture.Create<string>(),
            PasswordSalt = fixture.Create<string>(),
            UserRoleId = (int)fixture.Create<UserRole>(),
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>(),
            IsDeleted = fixture.Create<bool>(),
            DeletedBy = fixture.Create<string>(),
            DeletedAt = fixture.Create<DateTime>()
        };
    }
}