using System.Linq.Expressions;
using AutoMapper;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class PagedRepository<T> : IPagedRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly IMapper _mapper;

    public PagedRepository(DbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public abstract List<string> GetSortableFields();
    public abstract List<string> GetSearchableFields();

    public async Task<int> GetCountAsync(PagedRequest pagedRequest)
    {
        var query = _context.Set<T>().AsQueryable();

        if (pagedRequest.SearchField != null && !string.IsNullOrWhiteSpace(pagedRequest.SearchTerm?.ToString()))
        {
            query = ApplySearch(query, pagedRequest);
        }

        return await query.CountAsync();
    }

    public async Task<PagedResponse<TModel>> GetAllAsync<TModel>(PagedRequest pagedRequest)
        where TModel : class
    {
        var totalCount = await GetCountAsync(pagedRequest);
        var pageCount = ((totalCount == 0 ? 1 : totalCount) + pagedRequest.PageSize - 1) / pagedRequest.PageSize;

        var query = _context.Set<T>().AsQueryable();

        if (pagedRequest.SearchField != null && !string.IsNullOrWhiteSpace(pagedRequest.SearchTerm?.ToString()))
        {
            query = ApplySearch(query, pagedRequest);
        }

        query = ApplySorting(query, pagedRequest);

        var entities = await query
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize)
            .ToListAsync();

        var data = _mapper.Map<List<TModel>>(entities);

        return new PagedResponse<TModel>
        {
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize,
            PageCount = new PageCount(pageCount),
            TotalCount = new TotalCount(totalCount),
            HasMore = new HasMore(pagedRequest.PageNumber < pageCount),
            Data = data
        };
    }

    private IQueryable<T> ApplySearch(IQueryable<T> query, PagedRequest pagedRequest)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, pagedRequest.SearchField!);
        var toStringCall = Expression.Call(property, "ToString", null);
        var toLowerCall = Expression.Call(toStringCall, "ToLower", null);
        var searchValue = Expression.Constant(pagedRequest.SearchTerm.ToString()!.ToLower());
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
        var containsExpression = Expression.Call(toLowerCall, containsMethod!, searchValue);

        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        return query.Where(lambda);
    }

    private IQueryable<T> ApplySorting(IQueryable<T> query, PagedRequest pagedRequest)
    {
        if (pagedRequest.SortBy != null)
        {
            var sortByProperty = typeof(T).GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, pagedRequest.SortBy, StringComparison.OrdinalIgnoreCase));

            if (sortByProperty != null)
            {
                return pagedRequest.SortOrder is null or SortOrder.Asc
                    ? query.OrderBy(x => EF.Property<object>(x, sortByProperty.Name))
                    : query.OrderByDescending(x => EF.Property<object>(x, sortByProperty.Name));
            }
        }

        var defaultProperty = typeof(T).GetProperties().FirstOrDefault();
        if (defaultProperty != null)
        {
            return pagedRequest.SortOrder is null or SortOrder.Asc
                ? query.OrderBy(x => EF.Property<object>(x, defaultProperty.Name))
                : query.OrderByDescending(x => EF.Property<object>(x, defaultProperty.Name));
        }

        return query;
    }
}