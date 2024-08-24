using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Common;

namespace Presentation.Mappers;

public static class PagedRequestMapper
{
    private const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 10;

    public static PagedRequest Map(PagedRequestDto pagedRequestDto)
    {
        return new PagedRequest
        {
            PageNumber = pagedRequestDto.PageNumber is null ? new PageNumber(DefaultPageNumber) : new PageNumber(pagedRequestDto.PageNumber.Value),
            PageSize = pagedRequestDto.PageSize is null ? new PageSize(DefaultPageSize) : new PageSize(pagedRequestDto.PageSize.Value)
        };
    }
}