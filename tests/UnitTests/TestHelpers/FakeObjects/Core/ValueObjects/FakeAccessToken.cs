using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeAccessToken
{
    public static AccessToken CreateValid(IFixture fixture)
    {
        return new AccessToken(fixture.Create<string>());
    }
}