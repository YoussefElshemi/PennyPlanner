using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePasswordResetToken
{
    public static PasswordResetToken CreateValid(IFixture fixture)
    {
        return new PasswordResetToken(fixture.Create<string>());
    }
}