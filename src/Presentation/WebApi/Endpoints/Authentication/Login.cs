using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Mappers;
using Presentation.WebApi.Models.Authentication;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.Authentication;

public class Login(IAuthenticationService authenticationService,
    IValidator<LoginRequestDto> validator) : Endpoint<LoginRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.Login);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<LoginRequestDto>(MediaTypeNames.Application.Json)
            .Produces<AuthenticationResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.Login;
            s.Description = SwaggerSummaries.Authentication.Login;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(loginRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var authenticationRequest = AuthenticationRequestMapper.Map(loginRequestDto, ipAddress);

        var authenticationResponse = await authenticationService.AuthenticateAsync(authenticationRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}