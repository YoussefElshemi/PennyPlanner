using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Models.User.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.User;

public class ChangePassword(IAuthenticationService authenticationService,
    IMapper mapper) : Endpoint<ChangePasswordRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Patch(ApiUrls.User.ChangePassword);
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
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(ChangePasswordRequestDto changePasswordRequestDto, CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        var validator = new ChangePasswordRequestDtoValidator(authenticationService, user!);
        await validator.ValidateAndThrowAsync(changePasswordRequestDto, cancellationToken);

        var changePasswordRequest = mapper.Map<ChangePasswordRequest>(changePasswordRequestDto);

        var updatedUser = await authenticationService.ChangePasswordAsync(user!, changePasswordRequest.Password);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendOkAsync(response, cancellation: cancellationToken);
    }
}