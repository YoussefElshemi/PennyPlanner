using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeUsername
{
    internal const string Valid = "username";
    internal const string Invalid = "x";

    public static Username CreateValid()
    {
        return new Username(Valid);
    }
}