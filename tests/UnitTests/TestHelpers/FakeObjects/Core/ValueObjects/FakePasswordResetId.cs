using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePasswordResetId
{
    public static PasswordResetId CreateValid(IFixture fixture)
    {
        return new PasswordResetId(fixture.Create<Guid>());
    }
}