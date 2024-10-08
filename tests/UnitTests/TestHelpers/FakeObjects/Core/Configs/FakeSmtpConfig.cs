using AutoFixture;
using Core.Configs;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Configs;

public static class FakeSmtpConfig
{
    public static SmtpConfig CreateValid(IFixture fixture)
    {
        return new SmtpConfig
        {
            Host = fixture.Create<string>(),
            Port = fixture.Create<int>(),
            EmailAddress = FakeEmailAddress.Valid,
            Name = fixture.Create<string>(),
            Password = fixture.Create<string>(),
            NumberOfRetries = fixture.Create<int>()
        };
    }
}