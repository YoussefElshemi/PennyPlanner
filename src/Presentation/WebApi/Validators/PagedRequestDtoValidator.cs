using Core.Interfaces.Repositories;
using FluentValidation;
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
            .LessThanOrEqualTo(x => (repository.GetCountAsync().Result + x.PageSize - 1) / x.PageSize)
            .When(x => x.PageSize > 0);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(MaxPageSize);
    }
}