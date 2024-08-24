using Core.ValueObjects;
using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Presentation.WebApi.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class PagedRequestMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenPagedRequestDto_ReturnsPageRequest()
    {
        // Arrange
        var pagedRequestDto = FakePagedRequestDto.CreateValid(Fixture);

        // Act
        var pagedRequest = PagedRequestMapper.Map(pagedRequestDto);

        // Assert
        pagedRequest.PageSize.Should().Be(new PageSize(pagedRequestDto.PageSize!.Value));
        pagedRequest.PageNumber.Should().Be(new PageNumber(pagedRequestDto.PageNumber!.Value));
    }
}