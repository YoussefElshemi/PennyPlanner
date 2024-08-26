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

public class Register(
    IAuthenticationService authenticationService,
    IValidator<RegisterRequestDto> validator,
    IMapper mapper) : Endpoint<RegisterRequestDto, AuthenticationResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.Register);
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
            s.ExampleRequest = ExampleRequests.Authentication.Register;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RegisterRequestDto registerRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(registerRequestDto, cancellationToken);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var createUserRequest = mapper.Map<CreateUserRequest>(registerRequestDto, opt => { opt.Items["IpAddress"] = ipAddress; });

        var authenticationResponse = await authenticationService.CreateAsync(createUserRequest);

        var response = mapper.Map<AuthenticationResponseDto>(authenticationResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}