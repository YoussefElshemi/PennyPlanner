using AutoMapper;
using Core.Models;
using Presentation.WebApi.Models.Common;
using IPagedResponseMapper = Presentation.Mappers.Interfaces.IPagedResponseMapper;

namespace Presentation.Mappers;

public class PagedResponseMapper(IMapper mapper) : IPagedResponseMapper
{
    public PagedResponseDto<TDto> Map<TModel, TDto>(PagedResponse<TModel> pagedResponse)
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
            Data = pagedResponse.Data.Select(mapper.Map<TModel, TDto>).ToArray()
        };
    }
}