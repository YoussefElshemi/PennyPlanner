using System.Net;
using System.Net.Mime;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Models.UserManagement;
using Presentation.WebApi.Models.UserManagement.Validators;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class DeleteUser(
    IUserService userService,
    IUserRepository userRepository) : Endpoint<UserRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Delete(ApiRoutes.UserManagement.DeleteUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<UserRequestDto>(MediaTypeNames.Application.Json)
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
            s.ExampleRequest = ExampleRequests.UserManagement.DeleteUser;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(UserRequestDto userRequestDto, CancellationToken cancellationToken)
    {
        var authenticatedUser = HttpContext.Items["User"] as User;

        var validator = new DeleteUserRequestDtoValidator(userRepository, authenticatedUser!);
        await validator.ValidateAndThrowAsync(userRequestDto, cancellationToken);

        await userService.DeleteAsync(new UserId(userRequestDto.UserId), authenticatedUser!.Username);

        await SendNoContentAsync(cancellationToken);
    }
}