using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailSubject
{
    public static EmailSubject CreateValid(IFixture fixture)
    {
        return new EmailSubject(fixture.Create<string>());
    }
}