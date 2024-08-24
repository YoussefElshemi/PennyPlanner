using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeLoginId
{
    public static LoginId CreateValid(IFixture fixture)
    {
        return new LoginId(fixture.Create<Guid>());
    }
}