namespace Presentation.WebApi.Email.Models;

public record SendEmailRequestDto
{
    public required string EmailAddress { get; init; }
    public required string EmailSubject { get; init; }
    public required string EmailBody { get; init; }
}