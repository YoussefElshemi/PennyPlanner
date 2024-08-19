using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailAddress
{
    private const string Valid = "valid@valid.com";

    public static EmailAddress CreateValid()
    {
        return new EmailAddress(Valid);
    }
}