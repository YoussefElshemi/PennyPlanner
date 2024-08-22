using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PasswordResetEntityConfiguration : IEntityTypeConfiguration<PasswordResetEntity>
{
    private const string TableName = "PasswordResets";

    public void Configure(EntityTypeBuilder<PasswordResetEntity> builder)
    {
        builder.HasKey(x => x.PasswordResetId);

        builder
            .HasOne(e => e.UserEntity)
            .WithMany(e => e.PasswordResets)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.ToTable(TableName);
    }
}