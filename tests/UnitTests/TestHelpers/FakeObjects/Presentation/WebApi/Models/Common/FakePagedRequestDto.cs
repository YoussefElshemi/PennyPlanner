using AutoFixture;
using Presentation.WebApi.Models.Common;

namespace UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models.Common;

public static class FakePagedRequestDto
{
    public static PagedRequestDto CreateValid(IFixture fixture)
    {
        return new PagedRequestDto
        {
            PageNumber = fixture.Create<int>(),
            PageSize = fixture.Create<int>()
        };
    }
}