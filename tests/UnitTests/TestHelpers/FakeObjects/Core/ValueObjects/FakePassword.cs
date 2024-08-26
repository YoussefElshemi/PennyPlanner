using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePassword
{
    internal const string Valid = "P@$$w0rd";
    internal const string Invalid = "x";

    public static Password CreateValid()
    {
        return new Password(Valid);
    }
}