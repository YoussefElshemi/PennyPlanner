using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Mappers;
using Presentation.WebApi.Models.Common;
using Presentation.WebApi.Models.Common.Validators;
using Presentation.WebApi.Models.User;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class GetUsers(IUserService userService,
    IUserRepository userRepository) : Endpoint<PagedRequestDto, PagedResponseDto<UserProfileResponseDto>>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiUrls.UserManagement.GetUsers);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<PagedRequestDto>(MediaTypeNames.Application.Json)
            .Produces<PagedResponseDto<UserProfileResponseDto>>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.GetUsers;
            s.Description = SwaggerSummaries.UserManagement.GetUsers;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        var validator = new PagedRequestDtoValidator<Core.Models.User>(userRepository);
        await validator.ValidateAndThrowAsync(pagedRequestDto, cancellationToken);

        var pagedRequest = PagedRequestMapper.Map(pagedRequestDto);

        var pagedResponse = await userService.GetAllAsync(pagedRequest);

        var pagedResponseDto = PagedResponseMapper.Map(pagedResponse, UserProfileResponseMapper.Map);

        await SendAsync(pagedResponseDto, cancellation: cancellationToken);
    }
}