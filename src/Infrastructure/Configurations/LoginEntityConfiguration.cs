using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class LoginEntityConfiguration : IEntityTypeConfiguration<LoginEntity>
{
    private const string TableName = "Logins";

    public void Configure(EntityTypeBuilder<LoginEntity> builder)
    {
        builder.HasKey(x => x.LoginId);

        builder
            .HasOne(e => e.UserEntity)
            .WithMany(e => e.Logins)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.ToTable(TableName);
    }
}