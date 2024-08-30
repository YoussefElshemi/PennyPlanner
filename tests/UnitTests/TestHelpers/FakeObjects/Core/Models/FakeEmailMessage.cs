using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakeEmailMessage
{
    public static EmailMessage CreateValid(IFixture fixture)
    {
        return new EmailMessage
        {
            EmailId = FakeEmailId.CreateValid(fixture),
            EmailAddress = FakeEmailAddress.CreateValid(),
            EmailSubject = FakeEmailSubject.CreateValid(fixture),
            EmailBody = FakeEmailBody.CreateValid(fixture),
            IsProcessed = FakeIsProcessed.CreateValid(fixture),
            CreatedBy = FakeUsername.CreateValid(),
            CreatedAt = FakeCreatedAt.CreateValid(fixture),
            UpdatedBy = FakeUsername.CreateValid(),
            UpdatedAt = FakeUpdatedAt.CreateValid(fixture)
        };
    }
}