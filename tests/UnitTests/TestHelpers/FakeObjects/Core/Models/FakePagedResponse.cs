using AutoFixture;
using Core.Models;
using UnitTests.TestHelpers.FakeObjects.Core.ValueObjects;

namespace UnitTests.TestHelpers.FakeObjects.Core.Models;

public static class FakePagedResponse<T>
{
    public static PagedResponse<T> CreateValid(IFixture fixture)
    {
        return new PagedResponse<T>
        {
            PageNumber = FakePageNumber.CreateValid(fixture),
            PageSize = FakePageSize.CreateValid(fixture),
            PageCount = FakePageCount.CreateValid(fixture),
            TotalCount = FakeTotalCount.CreateValid(fixture),
            HasMore = FakeHasMore.CreateValid(fixture),
            Data = [fixture.Create<T>()]
        };
    }
}