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
        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.EmailAddress).IsUnique();

        builder
            .HasOne(e => e.UserRoleEntity)
            .WithMany(e => e.Users)
            .HasForeignKey(x => x.UserRoleId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder
            .HasMany(e => e.PasswordResets)
            .WithOne(e => e.UserEntity)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(e => e.Logins)
            .WithOne(e => e.UserEntity)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.ToTable(TableName);

        builder.HasData(SeedObjects.Users);
    }
}