using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    private const string TableName = "Users";

    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.UserId);

        builder
            .HasOne(e => e.UserRoleEntity)
            .WithMany(e => e.Users)
            .HasForeignKey(x => x.UserRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.ToTable(TableName);

        builder.HasData(SeedObjects.Users);
    }
}