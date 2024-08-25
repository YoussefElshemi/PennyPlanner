using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Models.UserManagement;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class DeleteUser(IUserService userService,
    IValidator<GetUserRequestDto> validator) : Endpoint<GetUserRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Delete(ApiRoutes.UserManagement.DeleteUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<GetUserRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.NoContent)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.DeleteUser;
            s.Description = SwaggerSummaries.UserManagement.DeleteUser;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(GetUserRequestDto getUserRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(getUserRequestDto, cancellationToken);

        var authenticatedUser = HttpContext.Items["User"] as User;

        await userService.DeleteAsync(new UserId(getUserRequestDto.UserId), authenticatedUser!.Username);

        await SendNoContentAsync(cancellation: cancellationToken);
    }
}