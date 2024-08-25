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
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.Authentication;

public class ResetPassword(IAuthenticationService authenticationService,
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
            s.Description = SwaggerSummaries.Authentication.ResetPassword;
            s.ExampleRequest = ExampleRequests.Authentication.ResetPassword;
        });

        Options(x => x.WithTags(SwaggerTags.Authentication));
    }

    public override async Task HandleAsync(ResetPasswordRequestDto requestResetPasswordRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(requestResetPasswordRequestDto, cancellationToken);

        var resetPasswordReset = mapper.Map<ResetPasswordRequest>(requestResetPasswordRequestDto);

        await authenticationService.ResetPassword(resetPasswordReset);

        await SendNoContentAsync(cancellationToken);
    }
}