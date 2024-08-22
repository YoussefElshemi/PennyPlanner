using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    private const string TableName = "UserRoles";

    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.HasKey(x => x.UserRoleId);

        builder
            .HasMany(x => x.Users)
            .WithOne(x => x.UserRoleEntity)
            .HasForeignKey(x => x.UserRoleId);

        builder.ToTable(TableName);

        builder.HasData(SeedObjects.UserRoles);
    }
}