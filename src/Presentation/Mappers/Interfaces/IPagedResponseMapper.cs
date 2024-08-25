using Core.Models;
using Presentation.WebApi.Models.Common;

namespace Presentation.Mappers.Interfaces;

public interface IPagedResponseMapper
{
    PagedResponseDto<TDto> Map<TModel, TDto>(PagedResponse<TModel> pagedResponse);
}