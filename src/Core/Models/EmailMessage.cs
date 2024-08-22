using Core.ValueObjects;

namespace Core.Models;

public record EmailMessage
{
    public required EmailAddress EmailAddress { get; init; }
    public required EmailSubject EmailSubject { get; init; }
    public required EmailBody EmailBody { get; init; }
}