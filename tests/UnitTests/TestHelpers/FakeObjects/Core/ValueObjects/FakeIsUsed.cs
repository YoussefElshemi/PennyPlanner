using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakeIsUsed
{
    public static IsUsed CreateValid()
    {
        return new IsUsed(false);
    }
}