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

public class Register(IAuthenticationService authenticationService,
    IValidator<RegisterRequestDto> validator) : Endpoint<RegisterRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiUrls.AuthenticationUrls.Register);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<RegisterRequestDto>(MediaTypeNames.Application.Json)
            .Produces<AuthenticationResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.Register;
            s.Description = SwaggerSummaries.Authentication.Register;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(registerRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var createUserRequest = CreateUserRequestMapper.Map(registerRequestDto, ipAddress);

        var authenticationResponse = await authenticationService.CreateAsync(createUserRequest);

        var response = AuthenticationResponseMapper.Map(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}