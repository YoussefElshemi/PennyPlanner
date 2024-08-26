using AutoMapper;
using Core.Models;
using FluentAssertions;
using Presentation.Mappers;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using UnitTests.TestHelpers;
using UnitTests.TestHelpers.FakeObjects.Core.Models;

namespace UnitTests.Tests.Presentation.Mappers;

public class PagedResponseMapperTests : BaseTestClass
{
    private readonly IMapper _mapper;
    private readonly PagedResponseMapper _pagedResponseMapper;

    public PagedResponseMapperTests()
    {
        var mapperConfig = new MapperConfiguration(x => x.AddProfile<PresentationProfile>());
        _mapper = mapperConfig.CreateMapper();
        _pagedResponseMapper = new PagedResponseMapper(_mapper);
    }

    [Fact]
    public void Map_GivenPagedResponseDto_ReturnsPageResponse()
    {
        // Arrange
        var pagedResponse = FakePagedResponse.CreateValid(Fixture);

        // Act
        var pagedResponseDto = _pagedResponseMapper.Map<User, UserProfileResponseDto>(pagedResponse);

        // Assert
        pagedResponseDto.Metadata.PageNumber.Should().Be(pagedResponse.PageNumber);
        pagedResponseDto.Metadata.PageSize.Should().Be(pagedResponse.PageSize);
        pagedResponseDto.Metadata.TotalCount.Should().Be(pagedResponse.TotalCount);
        pagedResponseDto.Metadata.PageCount.Should().Be(pagedResponse.PageCount);
        pagedResponseDto.Metadata.HasMore.Should().Be(pagedResponse.HasMore);
        pagedResponseDto.Data.Should().BeEquivalentTo(pagedResponse.Data.Select(_mapper.Map<User, UserProfileResponseDto>));
    }
}