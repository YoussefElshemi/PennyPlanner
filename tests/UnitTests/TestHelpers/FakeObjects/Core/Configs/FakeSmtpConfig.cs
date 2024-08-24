using AutoFixture;
using Core.Configs;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeSmtpConfig
{
    public static SmtpConfig CreateValid(IFixture fixture)
    {
        return new SmtpConfig
        {
            Host = fixture.Create<string>(),
            Port = fixture.Create<int>(),
            EmailAddress = fixture.Create<string>(),
            Name = fixture.Create<string>(),
            Password = fixture.Create<string>()
        };
    }
}