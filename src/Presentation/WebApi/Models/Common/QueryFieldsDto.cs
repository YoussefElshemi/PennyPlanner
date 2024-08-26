namespace Presentation.WebApi.Models.Common;

public record QueryFieldsDto
{
    public required string[] Fields { get; init; }
}