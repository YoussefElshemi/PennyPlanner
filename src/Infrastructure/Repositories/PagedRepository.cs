using System.Linq.Expressions;
using AutoMapper;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class PagedRepository<T>(DbContext context, IMapper mapper) : IPagedRepository<T>
    where T : class
{
    public abstract IDictionary<string, string> GetSortableFields();
    public abstract IDictionary<string, string> GetSearchableFields();

    public async Task<int> GetCountAsync(PagedRequest pagedRequest)
    {
        var query = context.Set<T>().AsQueryable();
        query = FilterDeleted(query);

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

        var query = context.Set<T>().AsQueryable();
        query = FilterDeleted(query);

        if (pagedRequest.SearchField != null && !string.IsNullOrWhiteSpace(pagedRequest.SearchTerm?.ToString()))
        {
            query = ApplySearch(query, pagedRequest);
        }

        query = ApplySorting(query, pagedRequest);

        var entities = await query
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize)
            .ToListAsync();

        var data = mapper.Map<List<TModel>>(entities);

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

    private static IQueryable<T> FilterDeleted(IQueryable<T> query)
    {
        if (typeof(T).GetProperties().All(x => x.Name != "IsDeleted"))
        {
            return query;
        }

        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, "IsDeleted");
        var equalsExpression = Expression.Equal(property, Expression.Constant(false));

        var lambda = Expression.Lambda<Func<T, bool>>(equalsExpression, parameter);
        return query.Where(lambda);
    }

    private static IQueryable<T> ApplySearch(IQueryable<T> query, PagedRequest pagedRequest)
    {
        var parameter = Expression.Parameter(typeof(T), "e");
        var property = Expression.Property(parameter, pagedRequest.SearchField!);
        var toStringCall = Expression.Call(property, "ToString", null);
        var toLowerCall = Expression.Call(
            property.Type == typeof(string)
                ? property
                : toStringCall,
            "ToLower", null);
        var searchValue = Expression.Constant(pagedRequest.SearchTerm.ToString()!.ToLower());
        var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
        var containsExpression = Expression.Call(toLowerCall, containsMethod!, searchValue);

        var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
        return query.Where(lambda);
    }

    private static IQueryable<T> ApplySorting(IQueryable<T> query, PagedRequest pagedRequest)
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