namespace Infrastructure.Entities;

public record EmailMessageOutboxEntity : BaseEntity
{
    public required Guid EmailId { get; init; }
    public required string EmailAddress { get; init; }
    public required string EmailSubject { get; init; }
    public required string EmailBody { get; init; }
    public required bool IsProcessed { get; set; }
}