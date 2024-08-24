using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIsUsed
{
    public static IsUsed CreateValid(IFixture fixture)
    {
        return new IsUsed(fixture.Create<bool>());
    }
}