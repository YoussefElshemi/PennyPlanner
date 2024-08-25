using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Extensions;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RequestResetPassword(IAuthenticationService authenticationService,
    IValidator<RequestResetPasswordRequestDto> validator) : Endpoint<RequestResetPasswordRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.RequestResetPassword);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<RequestResetPasswordRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Accepted)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.RequestResetPassword;
            s.Description = SwaggerSummaries.Authentication.RequestResetPassword;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RequestResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var requestResetPasswordRequest = RequestResetPasswordRequestMapper.Map(requestResetPasswordRequestDto);

        await authenticationService.RequestResetPassword(requestResetPasswordRequest);

        await this.SendAccepted(cancellationToken);
    }
}