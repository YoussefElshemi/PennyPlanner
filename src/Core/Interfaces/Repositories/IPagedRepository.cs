using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPagedRepository<_>
{
    Task<int> GetCountAsync(PagedRequest pagedRequest);
    Task<PagedResponse<TModel>> GetAllAsync<TModel>(PagedRequest pagedRequest) where TModel : class;
    IDictionary<string, string> GetSortableFields();
    IDictionary<string, string> GetSearchableFields();
}