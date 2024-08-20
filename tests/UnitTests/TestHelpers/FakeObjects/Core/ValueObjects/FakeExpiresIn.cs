using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeExpiresIn
{
    public static ExpiresIn CreateValid(IFixture fixture)
    {
        return new ExpiresIn(fixture.Create<int>());
    }
}