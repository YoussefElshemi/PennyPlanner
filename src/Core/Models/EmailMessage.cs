using Core.ValueObjects;

namespace Core.Models;

public record EmailMessage
{
    public required EmailId EmailId { get; init; }
    public required EmailAddress EmailAddress { get; init; }
    public required EmailSubject EmailSubject { get; init; }
    public required EmailBody EmailBody { get; init; }
    public required IsProcessed IsProcessed { get; init; }
    public required Username CreatedBy { get; init; }
    public required CreatedAt CreatedAt { get; init; }
    public required Username UpdatedBy { get; init; }
    public required UpdatedAt UpdatedAt { get; init; }
}