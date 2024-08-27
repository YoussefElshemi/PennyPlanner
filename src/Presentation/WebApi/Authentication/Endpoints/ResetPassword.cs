using System.Net;
using System.Net.Mime;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Authentication.Models.Requests;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Authentication.Endpoints;

public class ResetPassword(
    IAuthenticationService authenticationService,
    IValidator<ResetPasswordRequestDto> validator,
    IMapper mapper) : Endpoint<ResetPasswordRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.ResetPassword);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<ResetPasswordRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.NoContent)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.ResetPassword;
            s.ExampleRequest = ExampleRequests.Authentication.ResetPassword;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(ResetPasswordRequestDto requestPasswordResetRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(requestPasswordResetRequestDto, cancellationToken);

        var resetPasswordReset = mapper.Map<ResetPasswordRequest>(requestPasswordResetRequestDto);

        await authenticationService.ResetPassword(resetPasswordReset);

        await SendNoContentAsync(cancellationToken);
    }
}