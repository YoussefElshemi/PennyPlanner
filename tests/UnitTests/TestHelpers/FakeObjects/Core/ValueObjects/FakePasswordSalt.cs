using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePasswordSalt
{
    public static PasswordSalt CreateValid(IFixture fixture)
    {
        return new PasswordSalt(Convert.ToBase64String(fixture.CreateMany<byte>(32).ToArray()));
    }
}