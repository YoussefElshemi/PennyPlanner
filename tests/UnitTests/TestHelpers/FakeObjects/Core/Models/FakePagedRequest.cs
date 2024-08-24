using AutoFixture;
using Core.Models;
using Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakePagedRequest
{
    public static PagedRequest CreateValid(IFixture fixture)
    {
        return new PagedRequest
        {
            PageNumber = new PageNumber(fixture.Create<int>()),
            PageSize = new PageSize(fixture.Create<int>())
        };
    }
}