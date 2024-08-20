using AutoFixture;
using Core.Extensions;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeAccessToken
{
    public static AccessToken CreateValid(IFixture fixture)
    {
        return new AccessToken(fixture.Create<string>());
    }
}