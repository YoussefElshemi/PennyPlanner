using System.Linq.Expressions;
using AutoMapper;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(
    PennyPlannerDbContext context,
    IMapper mapper) : IUserRepository
{
    public async Task<PagedResponse<User>> GetAllAsync(PagedRequest pagedRequest)
    {
        var totalCount = await GetCountAsync(pagedRequest);
        var pageCount = ((totalCount == 0 ? 1 : totalCount) + pagedRequest.PageSize - 1) / pagedRequest.PageSize;

        var query = context.Users.AsQueryable()
            .Where(x => !x.IsDeleted);

        if (pagedRequest.SearchField != null && !string.IsNullOrWhiteSpace(pagedRequest.SearchTerm))
        {
            var parameter = Expression.Parameter(typeof(UserEntity), "e");
            var property = Expression.Property(parameter, pagedRequest.SearchField);
            var toStringCall = Expression.Call(property, "ToString", null);
            var toLowerCall = Expression.Call(toStringCall, "ToLower", null);
            var searchValue = Expression.Constant(pagedRequest.SearchTerm.Value.ToString().ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
            var containsExpression = Expression.Call(toLowerCall, containsMethod!, searchValue);

            var lambda = Expression.Lambda<Func<UserEntity, bool>>(containsExpression, parameter);
            query = query.Where(lambda);
        }

        if (pagedRequest.SortBy.HasValue)
        {
            var sortByProperty = typeof(UserEntity)
                .GetProperties()
                .FirstOrDefault(p => string.Equals(p.Name, pagedRequest.SortBy.ToString(), StringComparison.OrdinalIgnoreCase));

            query = pagedRequest.SortOrder is null or SortOrder.Asc
                ? query.OrderBy(x => EF.Property<object>(x, sortByProperty!.Name))
                : query.OrderByDescending(x => EF.Property<object>(x, sortByProperty!.Name));
        }
        else
        {
            query = pagedRequest.SortOrder is null or SortOrder.Asc
                ? query.OrderBy(x => x.UserId)
                : query.OrderByDescending(x => x.UserId);
        }

        var users = await query
            .Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize)
            .Take(pagedRequest.PageSize)
            .Select(entity => mapper.Map<User>(entity))
            .ToListAsync();

        return new PagedResponse<User>
        {
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize,
            PageCount = new PageCount(pageCount),
            TotalCount = new TotalCount(totalCount),
            HasMore = new HasMore(pagedRequest.PageNumber < pageCount),
            Data = users
        };
    }

    public List<string> GetSortableFields()
    {
        return
        [
            nameof(UserEntity.Username),
            nameof(UserEntity.EmailAddress),
            nameof(UserEntity.CreatedAt),
            nameof(UserEntity.UpdatedAt)
        ];
    }

    public List<string> GetSearchableFields()
    {
        return
        [
            nameof(UserEntity.Username),
            nameof(UserEntity.EmailAddress),
            nameof(UserEntity.CreatedBy),
            nameof(UserEntity.UpdatedBy)
        ];
    }

    public Task<int> GetCountAsync(PagedRequest pagedRequest)
    {
        var query = context.Users.AsQueryable();

        if (pagedRequest.SearchField != null && !string.IsNullOrWhiteSpace(pagedRequest.SearchTerm))
        {
            var parameter = Expression.Parameter(typeof(UserEntity), "e");
            var property = Expression.Property(parameter, pagedRequest.SearchField);
            var toStringCall = Expression.Call(property, "ToString", null);
            var toLowerCall = Expression.Call(toStringCall, "ToLower", null);
            var searchValue = Expression.Constant(pagedRequest.SearchTerm.Value.ToString().ToLower());
            var containsMethod = typeof(string).GetMethod("Contains", [typeof(string)]);
            var containsExpression = Expression.Call(toLowerCall, containsMethod!, searchValue);

            var lambda = Expression.Lambda<Func<UserEntity, bool>>(containsExpression, parameter);
            query = query.Where(lambda);
        }

        return query
            .Where(x => !x.IsDeleted)
            .CountAsync();
    }

    public Task<bool> ExistsByIdAsync(Guid userId)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.UserId == userId);
    }

    public async Task<User> GetByIdAsync(Guid userId)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.UserId == userId);

        return mapper.Map<User>(userEntity);
    }

    public Task<bool> ExistsByUsernameAsync(string username)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.Username == username);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.Username == username);

        return mapper.Map<User>(userEntity);
    }

    public Task<bool> ExistsByEmailAddressAsync(string emailAddress)
    {
        return context.Users
            .Where(x => !x.IsDeleted)
            .AnyAsync(u => u.EmailAddress == emailAddress);
    }

    public async Task<User> GetByEmailAddressAsync(string emailAddress)
    {
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .FirstAsync(u => u.EmailAddress == emailAddress);

        return mapper.Map<User>(userEntity);
    }

    public async Task CreateAsync(User user)
    {
        var userEntity = mapper.Map<UserEntity>(user);
        context.Users.Add(userEntity);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var userToUpdate = mapper.Map<UserEntity>(user);
        var userEntity = await context.Users
            .Where(x => !x.IsDeleted)
            .SingleAsync(x => x.UserId == userToUpdate.UserId);

        userEntity.Username = userToUpdate.Username;
        userEntity.EmailAddress = userToUpdate.EmailAddress;
        userEntity.PasswordHash = userToUpdate.PasswordHash;
        userEntity.PasswordSalt = userToUpdate.PasswordSalt;
        userEntity.UserRoleId = userToUpdate.UserRoleId;
        userEntity.UpdatedBy = userToUpdate.UpdatedBy;
        userEntity.UpdatedAt = userToUpdate.UpdatedAt;
        userEntity.IsDeleted = userToUpdate.IsDeleted;
        userEntity.DeletedBy = userToUpdate.DeletedBy;
        userEntity.DeletedAt = userToUpdate.DeletedAt;

        context.Users.Update(userEntity);
        await context.SaveChangesAsync();
    }
}