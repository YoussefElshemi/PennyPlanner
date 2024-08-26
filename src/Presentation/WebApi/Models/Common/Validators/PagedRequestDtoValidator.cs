using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models;
using Core.ValueObjects;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Results;
using Presentation.Mappers;

namespace Presentation.WebApi.Models.Common.Validators;

public class PagedRequestDtoValidator<T> : AbstractValidator<PagedRequestDto>
{
    private readonly IPagedRepository<T> _repository;
    public const int MaxPageSize = 100;

    internal const string InvalidSearchTermErrorMessage = $"{nameof(SearchTerm)} must be empty when {nameof(PagedRequestDto.SearchField)} is empty";
    internal const string InvalidSortOrderErrorMessage = $"{nameof(SortOrder)} must be {nameof(SortOrder.Asc)} or {nameof(SortOrder.Desc)}";
    internal readonly Func<List<string>, string> InvalidSortByErrorMessage = fields => $"{nameof(QueryField)} must be one of: {string.Join(", ", fields)}";
    internal readonly Func<List<string>, string> InvalidSearchFieldErrorMessage = fields => $"{nameof(PagedRequestDto.SearchField)} must be one of: {string.Join(", ", fields)}";

    public PagedRequestDtoValidator(IPagedRepository<T> repository)
    {
        _repository = repository;

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(MaxPageSize);

        RuleFor(x => x.SortOrder)
            .IsEnumName(typeof(SortOrder), false)
            .WithMessage(InvalidSortOrderErrorMessage)
            .When(x => !string.IsNullOrWhiteSpace(x.SortOrder));

        RuleFor(x => x.SortBy)
            .Must(x => _repository.GetSortableFields().Contains(x!, StringComparer.OrdinalIgnoreCase))
            .WithMessage(InvalidSortByErrorMessage(_repository.GetSortableFields()))
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

        RuleFor(x => x.SearchField)
            .Must(x => _repository.GetSearchableFields().Contains(x!, StringComparer.OrdinalIgnoreCase))
            .WithMessage(InvalidSearchFieldErrorMessage(_repository.GetSearchableFields()))
            .When(x => !string.IsNullOrWhiteSpace(x.SearchField))
            .DependentRules(() =>
            {
                RuleFor(x => x)
                    .CustomAsync(async (x, ctx, _) => await ValidatePageNumber(x, ctx));
            });

        RuleFor(x => x.SearchTerm)
            .Must(string.IsNullOrWhiteSpace)
            .When(x => string.IsNullOrWhiteSpace(x.SearchField))
            .WithMessage(InvalidSearchTermErrorMessage);
    }

    private async Task ValidatePageNumber(PagedRequestDto pagedRequestDto, ValidationContext<PagedRequestDto> context)
    {
        var pagedRequest = new PagedRequest
        {
            PageNumber = new PageNumber(1),
            PageSize = new PageSize(GetPageSize(pagedRequestDto)),
            SortBy = null,
            SortOrder = null,
            SearchField = !string.IsNullOrWhiteSpace(pagedRequestDto.SearchField) ? new QueryField(pagedRequestDto.SearchField) : null,
            SearchTerm = !string.IsNullOrWhiteSpace(pagedRequestDto.SearchTerm) ? new SearchTerm(pagedRequestDto.SearchTerm) : null
        };

        var count = await _repository.GetCountAsync(pagedRequest);
        var maxPageNumber = ((count == 0 ? 1 : count) + GetPageSize(pagedRequestDto) - 1) / GetPageSize(pagedRequestDto);

        if (pagedRequestDto.PageNumber > maxPageNumber)
        {
            context.MessageFormatter.AppendPropertyName(nameof(PageNumber));
            context.MessageFormatter.AppendArgument("ComparisonValue", maxPageNumber);

            var errorMessage = context.MessageFormatter.BuildMessage(
                new LanguageManager().GetString("LessThanOrEqualValidator"));

            context.AddFailure(new ValidationFailure(nameof(PageNumber), errorMessage)
            {
                ErrorCode = "LessThanOrEqualValidator"
            });
        }
    }

    private static int GetPageSize(PagedRequestDto x)
    {
        return x.PageSize is null or 0
            ? PresentationProfile.DefaultPageSize
            : x.PageSize.Value;
    }
}