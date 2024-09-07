using System.Net;
using System.Net.Mime;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Models.Responses;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using TwoFactorRequest = Core.Models.TwoFactorRequest;

namespace Presentation.WebApi.Authentication.Endpoints;

public class TwoFactor(
    IAuthenticationService authenticationService,
    IValidator<TwoFactorRequestDto> validator,
    IMapper mapper) : Endpoint<TwoFactorRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.TwoFactor);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<TwoFactorRequestDto>(MediaTypeNames.Application.Json)
            .Produces<AuthenticationResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.TwoFactor;
            s.ExampleRequest = ExampleRequests.Authentication.TwoFactor;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(TwoFactorRequestDto twoFactorRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(twoFactorRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var twoFactorRequest = mapper.Map<TwoFactorRequest>(twoFactorRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        var authenticationResponse = await authenticationService.TwoFactorAuthenticationAsync(twoFactorRequest);

        var response = mapper.Map<AuthenticationResponseDto>(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}