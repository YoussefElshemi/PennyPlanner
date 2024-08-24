using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePageNumber
{
    public static PageNumber CreateValid(IFixture fixture)
    {
        return new PageNumber(fixture.Create<int>());
    }
}