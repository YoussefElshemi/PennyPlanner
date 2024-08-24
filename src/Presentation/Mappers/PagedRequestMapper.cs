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
            PageNumber = new PageNumber(pagedRequestDto.PageNumber),
            PageSize = new PageSize(pagedRequestDto.PageSize)
        };
    }
}