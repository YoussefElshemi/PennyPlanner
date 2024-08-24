using Core.Models;

namespace Core.Interfaces.Repositories;

public interface IPagedRepository<T>
{
    Task<int> GetCountAsync();
    Task<PagedResponse<T>> GetAllAsync(PagedRequest pagedRequest);
}