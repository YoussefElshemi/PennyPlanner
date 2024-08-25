using Core.Enums;
using Core.Interfaces.Repositories;
using Core.ValueObjects;
using FluentValidation;
using Presentation.Mappers;

namespace Presentation.WebApi.Models.Common.Validators;

public class PagedRequestDtoValidator<T> : AbstractValidator<PagedRequestDto>
{
    public const int MaxPageSize = 100;

    internal const string InvalidSortOrderErrorMessage = $"Sort Order must be {nameof(SortOrder.Asc)} or {nameof(SortOrder.Desc)}";
    internal static readonly Func<List<string>, string> InvalidSortByErrorMessage = fields => $"{nameof(SortBy)} must be one of: {string.Join(", ", fields)}";

    public PagedRequestDtoValidator(IPagedRepository<T> repository)
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber)
            .LessThanOrEqualTo(x => (repository.GetCountAsync().Result + GetPageSize(x) - 1) / GetPageSize(x));

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(MaxPageSize);

        RuleFor(x => x.SortOrder)
            .IsEnumName(typeof(SortOrder), false)
            .WithMessage(InvalidSortOrderErrorMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.SortOrder));

        RuleFor(x => x.SortBy)
            .Must(x => repository.GetSortableFields().Contains(x!, StringComparer.OrdinalIgnoreCase))
            .WithMessage(InvalidSortByErrorMessage(repository.GetSortableFields()))
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));
    }

    private static int GetPageSize(PagedRequestDto x)
    {
        return x.PageSize is null or 0
            ? PresentationProfile.DefaultPageSize
            : x.PageSize.Value;
    }
}