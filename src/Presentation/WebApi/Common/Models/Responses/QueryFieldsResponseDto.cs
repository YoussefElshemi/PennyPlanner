using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApi.Common.Models.Responses;

/// <summary>
///     DTO for specifying fields that can be used for searching or sorting.
/// </summary>
public record QueryFieldsResponseDto
{
    /// <summary>
    ///     An array of field names that can be used for search or sorting operations.
    /// </summary>
    [Required]
    public required string[] Fields { get; init; }
}