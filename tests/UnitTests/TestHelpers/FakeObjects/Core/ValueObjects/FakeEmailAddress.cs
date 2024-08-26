using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeEmailAddress
{
    internal const string Valid = "valid@valid.com";
    internal const string Invalid = "invalid.invalid.com";

    public static EmailAddress CreateValid()
    {
        return new EmailAddress(Valid);
    }
}