using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeOneTimePasscodeId
{
    public static OneTimePasscodeId CreateValid(IFixture fixture)
    {
        return new OneTimePasscodeId(fixture.Create<Guid>());
    }
}