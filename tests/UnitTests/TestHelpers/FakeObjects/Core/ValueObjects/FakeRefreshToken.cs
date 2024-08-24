using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeRefreshToken
{
    public static RefreshToken CreateValid(IFixture fixture)
    {
        return new RefreshToken(fixture.Create<string>());
    }
}