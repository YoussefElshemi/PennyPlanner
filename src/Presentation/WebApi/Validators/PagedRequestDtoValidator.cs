using Core.Interfaces.Repositories;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.Common;

namespace Presentation.WebApi.Validators;

public class PagedRequestDtoValidator<T> : AbstractValidator<PagedRequestDto>
{
    public const int MaxPageSize = 100;

    public PagedRequestDtoValidator(IPagedRepository<T> repository)
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageNumber)
            .LessThanOrEqualTo(x => (repository.GetCountAsync().Result + GetPageSize(x) - 1) / GetPageSize(x));

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(MaxPageSize);
    }

    private static int GetPageSize(PagedRequestDto x)
    {
        return x.PageSize is null or 0
            ? PagedRequestMapper.DefaultPageSize
            : x.PageSize.Value;
    }
}