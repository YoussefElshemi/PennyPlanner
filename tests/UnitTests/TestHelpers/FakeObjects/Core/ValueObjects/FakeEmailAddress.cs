using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailAddress
{
    internal const string Valid = "valid@valid.com";

    public static EmailAddress CreateValid()
    {
        return new EmailAddress(Valid);
    }
}