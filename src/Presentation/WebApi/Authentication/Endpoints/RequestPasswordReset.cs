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
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Authentication.Endpoints;

public class RequestPasswordReset(IValidator<RequestPasswordResetRequestDto> validator,
    IAuthenticationService authenticationService,
    IMapper mapper) : Endpoint<RequestPasswordResetRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Authentication.RequestPasswordReset);
        AllowAnonymous();
        EnableAntiforgery();

        Description(b => b
            .Accepts<RequestPasswordResetRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.Accepted)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Authentication.RequestPasswordReset;
            s.ExampleRequest = ExampleRequests.Authentication.RequestPasswordReset;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(RequestPasswordResetRequestDto requestPasswordResetRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(requestPasswordResetRequestDto, cancellationToken);

        var requestPasswordResetRequest = mapper.Map<RequestPasswordResetRequest>(requestPasswordResetRequestDto);

        await authenticationService.RequestPasswordReset(requestPasswordResetRequest);

        await this.SendAccepted(cancellationToken);
    }
}