using AutoMapper;
using Core.Models;
using Presentation.WebApi.Models.Common;
using IPagedResponseMapper = Presentation.Mappers.Interfaces.IPagedResponseMapper;

namespace Presentation.Mappers;

public class PagedResponseMapper(IMapper mapper) : IPagedResponseMapper
{
    public PagedResponseDto<TSource> Map<TDestination, TSource>(PagedResponse<TDestination> pagedResponse)
    {
        return new PagedResponseDto<TSource>
        {
            Metadata = new PagedResponseMetadataDto
            {
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize,
                PageCount = pagedResponse.PageCount,
                TotalCount = pagedResponse.TotalCount,
                HasMore = pagedResponse.HasMore
            },
            Data = pagedResponse.Data.Select(mapper.Map<TDestination, TSource>).ToArray()
        };
    }
}