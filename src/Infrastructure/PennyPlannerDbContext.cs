using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PennyPlannerDbContext(DbContextOptions<PennyPlannerDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; init; }
    public DbSet<PasswordResetEntity> PasswordResets { get; init; }
    public DbSet<LoginEntity> Logins { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PennyPlannerDbContext).Assembly);

        var baseEntityProperties  = typeof(BaseEntity).GetProperties().Select(x => x.Name).ToList();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)) continue;

            var numberOfProperties = entityType.GetProperties().Count();
            foreach (var (property, i) in entityType.GetProperties().Select((property, i) => ( property, i )))
            {
                if (baseEntityProperties.Contains(property.Name))
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnOrder(numberOfProperties + i);
                }
                else
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasColumnOrder(i);
                }
            }
        }
    }
}