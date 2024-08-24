using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIpAddress
{
    public static IpAddress CreateValid(IFixture fixture)
    {
        return new IpAddress(fixture.Create<string>());
    }
}