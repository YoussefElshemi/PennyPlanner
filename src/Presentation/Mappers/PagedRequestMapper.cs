using Core.Models;
using Core.ValueObjects;
using Presentation.WebApi.Models.Common;

namespace Presentation.Mappers;

public static class PagedRequestMapper
{
    public static PagedRequest Map(PagedRequestDto pagedRequestDto)
    {
        return new PagedRequest
        {
            PageNumber = pagedRequestDto.PageNumber is null ? new PageNumber(1) : new PageNumber(pagedRequestDto.PageNumber.Value),
            PageSize = pagedRequestDto.PageSize is null ? new PageSize(10) : new PageSize(pagedRequestDto.PageSize.Value),
        };
    }
}