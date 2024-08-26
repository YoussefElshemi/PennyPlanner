using System.Net;
using System.Net.Mime;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Models.AuthenticatedUser;
using Presentation.WebApi.Models.AuthenticatedUser.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Endpoints.AuthenticatedUser;

public class ChangePassword(
    IAuthenticationService authenticationService,
    IMapper mapper) : Endpoint<ChangePasswordRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Patch(ApiRoutes.User.ChangePassword);
        EnableAntiforgery();

        Description(b => b
            .Accepts<ChangePasswordRequestDto>(MediaTypeNames.Application.Json)
            .Produces<UserProfileResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.User.ChangePassword;
            s.Description = SwaggerSummaries.User.ChangePassword;
            s.ExampleRequest = ExampleRequests.User.ChangePassword;
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(ChangePasswordRequestDto changePasswordRequestDto, CancellationToken cancellationToken)
    {
        var authenticatedUser = HttpContext.Items["User"] as User;

        var validator = new ChangePasswordRequestDtoValidator(authenticationService, authenticatedUser!);
        await validator.ValidateAndThrowAsync(changePasswordRequestDto, cancellationToken);

        var changePasswordRequest = mapper.Map<ChangePasswordRequest>(changePasswordRequestDto);

        var updatedUser = await authenticationService.ChangePasswordAsync(authenticatedUser!, changePasswordRequest.Password);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendOkAsync(response, cancellationToken);
    }
}