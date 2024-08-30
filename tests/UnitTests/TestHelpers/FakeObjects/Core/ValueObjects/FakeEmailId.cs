using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailId
{
    public static EmailId CreateValid(IFixture fixture)
    {
        return new EmailId(fixture.Create<Guid>());
    }
}