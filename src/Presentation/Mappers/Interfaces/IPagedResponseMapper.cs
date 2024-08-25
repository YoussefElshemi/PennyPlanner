using Core.Models;
using Presentation.WebApi.Models.Common;

namespace Presentation.Mappers.Interfaces;

public interface IPagedResponseMapper
{
    PagedResponseDto<TSource> Map<TDestination, TSource>(PagedResponse<TDestination> pagedResponse);
}