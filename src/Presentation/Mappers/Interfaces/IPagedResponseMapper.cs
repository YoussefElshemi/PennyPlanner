using Core.Models;
using Presentation.WebApi.Common.Models.Responses;

namespace Presentation.Mappers.Interfaces;

public interface IPagedResponseMapper
{
    PagedResponseDto<TSource> Map<TDestination, TSource>(PagedResponse<TDestination> pagedResponse);
}