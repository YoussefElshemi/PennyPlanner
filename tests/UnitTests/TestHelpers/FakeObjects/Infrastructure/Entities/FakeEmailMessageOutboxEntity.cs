using AutoFixture;
using Infrastructure.Entities;

namespace UnitTests.TestHelpers.FakeObjects.Infrastructure.Entities;

public static class FakeEmailMessageOutboxEntity
{
    public static EmailMessageOutboxEntity CreateValid(IFixture fixture)
    {
        return new EmailMessageOutboxEntity
        {
            EmailId = fixture.Create<Guid>(),
            EmailAddress = fixture.Create<string>(),
            EmailSubject = fixture.Create<string>(),
            EmailBody = fixture.Create<string>(),
            IsProcessed = fixture.Create<bool>(),
            CreatedBy = fixture.Create<string>(),
            CreatedAt = fixture.Create<DateTime>(),
            UpdatedBy = fixture.Create<string>(),
            UpdatedAt = fixture.Create<DateTime>()
        };
    }
}