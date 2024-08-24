using AutoFixture;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

public static class FakePageSize
{
    public static PageSize CreateValid(IFixture fixture)
    {
        return new PageSize(fixture.Create<int>());
    }
}