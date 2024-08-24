using Core.Models;
using Presentation.WebApi.Models.Common;

namespace Presentation.Mappers;

public static class PagedResponseMapper
{
    public static PagedResponseDto<TDto> Map<TModel, TDto>(PagedResponse<TModel> pagedResponse, Func<TModel, TDto> map)
    {
        return new PagedResponseDto<TDto>
        {
            Metadata = new PagedResponseMetadataDto {
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize,
                PageCount = pagedResponse.PageCount,
                TotalCount = pagedResponse.TotalCount,
                HasMore = pagedResponse.HasMore
            },
            Data = pagedResponse.Data.Select(map).ToArray()
        };
    }
}