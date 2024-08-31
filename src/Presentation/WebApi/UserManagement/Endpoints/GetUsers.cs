using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Mappers.Interfaces;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.Common.Models.Requests;
using Presentation.WebApi.Common.Models.Responses;
using Presentation.WebApi.Common.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.UserManagement.Endpoints;

public class GetUsers(
    IUserService userService,
    IUserRepository userRepository,
    IMapper mapper,
    IPagedResponseMapper pagedResponseMapper) : Endpoint<PagedRequestDto, PagedResponseDto<UserProfileResponseDto>>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.UserManagement.GetUsers);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Produces<PagedResponseDto<UserProfileResponseDto>>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.GetUsers;
            s.ExampleRequest = ExampleRequests.UserManagement.GetUsers;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        pagedRequestDto = MapValuesToDatabaseValues(pagedRequestDto);
        pagedRequestDto = MapPropertiesToDatabaseProperties(pagedRequestDto);

        var validator = new PagedRequestDtoValidator<User>(userRepository);
        await validator.ValidateAndThrowAsync(pagedRequestDto, cancellationToken);

        var pagedRequest = mapper.Map<PagedRequest>(pagedRequestDto);

        var pagedResponse = await userService.GetAllAsync(pagedRequest);

        var pagedResponseDto = pagedResponseMapper.Map<User, UserProfileResponseDto>(pagedResponse);

        await SendAsync(pagedResponseDto, cancellation: cancellationToken);
    }

    private PagedRequestDto MapPropertiesToDatabaseProperties(PagedRequestDto pagedRequestDto)
    {
        if (!string.IsNullOrWhiteSpace(pagedRequestDto.SearchField))
        {
            var searchableFields = userService.GetSearchableFields();
            if (searchableFields.TryGetValue(pagedRequestDto.SearchField, out var field))
            {
                pagedRequestDto = pagedRequestDto with
                {
                    SearchField = field,
                };
            }
        }

        if (!string.IsNullOrWhiteSpace(pagedRequestDto.SortBy))
        {
            var sortableFields = userService.GetSortableFields();
            if (sortableFields.TryGetValue(pagedRequestDto.SortBy, out var field))
            {
                pagedRequestDto = pagedRequestDto with
                {
                    SortBy = field
                };
            }
        }

        return pagedRequestDto;
    }

    private PagedRequestDto MapValuesToDatabaseValues(PagedRequestDto pagedRequestDto)
    {
        if (!string.IsNullOrWhiteSpace(pagedRequestDto.SearchField) && !string.IsNullOrWhiteSpace(pagedRequestDto.SearchTerm))
        {
            var mappableValues = userService.GetMappableValues();
            if (mappableValues.TryGetValue(pagedRequestDto.SearchField, out var values))
            {
                if (values.TryGetValue(pagedRequestDto.SearchTerm, out var value))
                {
                    pagedRequestDto = pagedRequestDto with
                    {
                        SearchTerm = value
                    };
                }
            }
        }

        return pagedRequestDto;
    }
}