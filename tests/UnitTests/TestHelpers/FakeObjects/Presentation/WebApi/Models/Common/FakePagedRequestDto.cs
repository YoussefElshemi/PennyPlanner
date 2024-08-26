using AutoFixture;
using Core.Enums;
using Presentation.WebApi.Models.Common;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Common;

public static class FakePagedRequestDto
{
    public static PagedRequestDto CreateValid(IFixture fixture)
    {
        return new PagedRequestDto
        {
            PageNumber = fixture.Create<int>(),
            PageSize = fixture.Create<int>(),
            SortBy = fixture.Create<string>(),
            SortOrder = fixture.Create<SortOrder>().ToString(),
            SearchTerm = fixture.Create<string>(),
            SearchField = fixture.Create<string>()
        };
    }
}