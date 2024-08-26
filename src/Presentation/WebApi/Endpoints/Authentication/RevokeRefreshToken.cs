using System.Net;
using System.Net.Mime;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Models.Authentication;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Endpoints.Authentication;

public class RevokeRefreshToken(
    IAuthenticationService authenticationService,
    IValidator<RefreshTokenRequestDto> validator,
    IMapper mapper) : Endpoint<RefreshTokenRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.RevokeRefreshToken);
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
            s.ExampleRequest = ExampleRequests.Authentication.RevokeRefreshToken;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RefreshTokenRequestDto refreshTokenRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(refreshTokenRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var refreshTokenRequest = mapper.Map<RefreshTokenRequest>(refreshTokenRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        await authenticationService.RevokeToken(refreshTokenRequest);

        await SendNoContentAsync(cancellationToken);
    }
}