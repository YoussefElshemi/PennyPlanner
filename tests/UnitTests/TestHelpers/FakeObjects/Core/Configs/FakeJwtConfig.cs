using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeJwtConfig
{
    public static JwtConfig CreateValid(IFixture fixture)
    {
        return new JwtConfig
        {
            Key = fixture.Create<string>(),
            Issuer = fixture.Create<string>(),
            Audience = fixture.Create<string>(),
            Lifetime = fixture.Create<int>()
        };
    }
}