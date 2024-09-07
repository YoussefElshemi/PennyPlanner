using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIsTwoFactorAuthenticationEnabled
{
    public static IsTwoFactorAuthenticationEnabled CreateValid(IFixture fixture)
    {
        return new IsTwoFactorAuthenticationEnabled(fixture.Create<bool>());
    }
}