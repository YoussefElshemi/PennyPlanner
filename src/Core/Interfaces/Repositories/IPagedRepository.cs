using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPagedRepository<T>
{
    Task<int> GetCountAsync(PagedRequest pagedRequest);
    Task<PagedResponse<T>> GetAllAsync(PagedRequest pagedRequest);
    List<string> GetSortableFields();
    List<string> GetSearchableFields();
}