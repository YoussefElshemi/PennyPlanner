using Core.ValueObjects;

namespace Core.Models;

public record EmailMessage
{
    public required EmailId EmailId { get; init; }
    public required EmailAddress EmailAddress { get; init; }
    public required EmailSubject EmailSubject { get; init; }
    public required EmailBody EmailBody { get; init; }
    public required IsProcessed IsProcessed { get; set; }
    public required Username CreatedBy { get; set; }
    public required CreatedAt CreatedAt { get; set; }
    public required Username UpdatedBy { get; set; }
    public required UpdatedAt UpdatedAt { get; set; }
}