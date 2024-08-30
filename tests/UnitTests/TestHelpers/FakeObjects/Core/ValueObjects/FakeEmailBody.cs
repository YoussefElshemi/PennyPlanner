using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailBody
{
    public static EmailBody CreateValid(IFixture fixture)
    {
        return new EmailBody(fixture.Create<string>());
    }
}