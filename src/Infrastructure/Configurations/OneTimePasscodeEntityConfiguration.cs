using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class OneTimePasscodeEntityConfiguration : IEntityTypeConfiguration<OneTimePasscodeEntity>
{
    private const string TableName = "OneTimePasscodes";

    public void Configure(EntityTypeBuilder<OneTimePasscodeEntity> builder)
    {
        builder.HasKey(x => x.OneTimePasscodeId);

        builder
            .HasOne(e => e.UserEntity)
            .WithMany(e => e.OneTimePasscodes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.ToTable(TableName);
    }
}