using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIsRevoked
{
    public static IsRevoked CreateValid(IFixture fixture)
    {
        return new IsRevoked(fixture.Create<bool>());
    }
}