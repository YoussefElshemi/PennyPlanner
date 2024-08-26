using AutoFixture;
using Core.Enums;
using Presentation.WebApi.Common.Models.Requests;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Common.Models.Requests;

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