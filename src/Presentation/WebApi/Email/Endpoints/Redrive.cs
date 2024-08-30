using System.Net;
using System.Net.Mime;
using Core.Enums;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.WebApi.Email.Models;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Email.Endpoints;

public class Redrive(
    IEmailService emailService,
    IValidator<RedriveEmailRequestDto> validator,
    IMapper mapper) : Endpoint<RedriveEmailRequestDto, EmailResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Post(ApiRoutes.Email.Redrive);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<RedriveEmailRequestDto>(MediaTypeNames.Application.Json)
            .Produces((int)HttpStatusCode.NoContent)
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Email.Redrive;
            s.ExampleRequest = ExampleRequests.Email.Redrive;
        });

        Options(x => x.WithTags(SwaggerTags.Email));
    }

    public override async Task HandleAsync(RedriveEmailRequestDto redriveEmailRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(redriveEmailRequestDto, cancellationToken);

        var emailResponse = await emailService.RedriveEmailAsync(new EmailId(redriveEmailRequestDto.EmailId));

        var response = mapper.Map<EmailResponseDto>(emailResponse);

        await SendAsync(response, cancellation: cancellationToken);
    }
}