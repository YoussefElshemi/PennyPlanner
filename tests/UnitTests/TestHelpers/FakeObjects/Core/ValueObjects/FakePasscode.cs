using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePasscode
{
    public static Passcode CreateValid(IFixture fixture)
    {
        return new Passcode(fixture.Create<string>());
    }
}