using AutoFixture;
using Core.Enums;
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
            PageSize = new PageSize(fixture.Create<int>()),
            SortBy = new QueryField(fixture.Create<string>()),
            SortOrder = fixture.Create<SortOrder>(),
            SearchField = new QueryField(fixture.Create<string>()),
            SearchTerm = new SearchTerm(fixture.Create<string>())
        };
    }
}