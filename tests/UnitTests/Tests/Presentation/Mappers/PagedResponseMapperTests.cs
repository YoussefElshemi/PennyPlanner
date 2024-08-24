using FluentAssertions;
using Presentation.Mappers;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class PagedResponseMapperTests : BaseTestClass
{
    [Fact]
    public void Map_GivenPagedResponseDto_ReturnsPageResponse()
    {
        // Arrange
        var pagedResponse = FakePagedResponse.CreateValid(Fixture);

        // Act
        var pagedResponseDto = PagedResponseMapper.Map(pagedResponse, o => o);

        // Assert
        pagedResponseDto.Metadata.PageNumber.Should().Be(pagedResponse.PageNumber);
        pagedResponseDto.Metadata.PageSize.Should().Be(pagedResponse.PageSize);
        pagedResponseDto.Metadata.TotalCount.Should().Be(pagedResponse.TotalCount);
        pagedResponseDto.Metadata.PageCount.Should().Be(pagedResponse.PageCount);
        pagedResponseDto.Metadata.HasMore.Should().Be(pagedResponse.HasMore);
        pagedResponseDto.Data.Should().BeEquivalentTo(pagedResponse.Data);
    }
}