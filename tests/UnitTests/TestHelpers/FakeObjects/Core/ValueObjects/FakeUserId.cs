using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeUserId
{
    public static UserId CreateValid(IFixture fixture)
    {
        return new UserId(fixture.Create<Guid>());
    }
}