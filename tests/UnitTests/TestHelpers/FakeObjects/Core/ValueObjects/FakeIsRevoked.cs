using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIsRevoked
{
    public static IsRevoked CreateValid()
    {
        return new IsRevoked(false);
    }
}