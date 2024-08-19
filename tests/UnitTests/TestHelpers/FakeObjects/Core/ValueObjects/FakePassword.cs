using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePassword
{
    public const string Valid = "P@$$w0rd";

    public static Password CreateValid()
    {
        return new Password(Valid);
    }
}