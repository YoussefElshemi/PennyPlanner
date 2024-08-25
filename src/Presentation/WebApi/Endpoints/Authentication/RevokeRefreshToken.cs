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

public class RevokeRefreshToken(IAuthenticationService authenticationService,
    IValidator<RefreshTokenRequestDto> validator) : Endpoint<RefreshTokenRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.RevokeRefreshToken);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<RefreshTokenRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.NoContent)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.RevokeRefreshToken;
            s.Description = SwaggerSummaries.Authentication.RevokeRefreshToken;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(refreshTokenRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var refreshTokenRequest = RefreshTokenRequestMapper.Map(refreshTokenRequestDto, ipAddress);

        await authenticationService.RevokeToken(refreshTokenRequest);

        await SendNoContentAsync(cancellation: cancellationToken);
    }
}