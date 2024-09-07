using System.Net;
using System.Net.Mime;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Extensions;
using Presentation.WebApi.Authentication.Models.Requests;
using Presentation.WebApi.Authentication.Models.Responses;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Authentication.Endpoints;

public class Login(
    IAuthenticationService authenticationService,
    IUserService userService,
    IValidator<LoginRequestDto> validator,
    IMapper mapper) : Endpoint<LoginRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.Login);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<LoginRequestDto>(MediaTypeNames.Application.Json)
            .Produces<AuthenticationResponseDto>()
            .Produces((int)HttpStatusCode.Accepted)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.Login;
            s.ExampleRequest = ExampleRequests.Authentication.Login;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(loginRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var authenticationRequest = mapper.Map<AuthenticationRequest>(loginRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        var user = await userService.GetAsync(authenticationRequest.Username);
        if (user.IsTwoFactorAuthenticationEnabled)
        {
            await authenticationService.RequestTwoFactorAsync(user, authenticationRequest);

            await this.SendAccepted(cancellationToken);
        }
        else
        {
            var authenticationResponse = await authenticationService.AuthenticateAsync(user, authenticationRequest);

            var response = mapper.Map<AuthenticationResponseDto>(authenticationResponse);

            await SendAsync(response, cancellation: cancellationToken);
        }
    }
}