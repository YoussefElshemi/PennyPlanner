using Infrastructure.Configurations;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class PennyPlannerDbContext(DbContextOptions<PennyPlannerDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PennyPlannerDbContext).Assembly);
    }
}