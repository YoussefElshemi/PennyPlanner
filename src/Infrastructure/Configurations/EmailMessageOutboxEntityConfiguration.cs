using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmailMessageOutboxEntityConfiguration : IEntityTypeConfiguration<EmailMessageOutboxEntity>
{
    private const string TableName = "EmailOutbox";

    public void Configure(EntityTypeBuilder<EmailMessageOutboxEntity> builder)
    {
        builder.HasKey(x => x.EmailId);

        builder.Property(x => x.RowVersion).IsRowVersion();

        builder.ToTable(TableName);
    }
}