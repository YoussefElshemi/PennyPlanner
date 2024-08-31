using System.Net;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Mappers.Interfaces;
using Presentation.WebApi.Common.Models.Requests;
using Presentation.WebApi.Common.Models.Responses;
using Presentation.WebApi.Common.Validators;
using Presentation.WebApi.Emails.Models;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.Emails.Endpoints;

public class GetEmails(
    IEmailService emailService,
    IEmailRepository emailRepository,
    IMapper mapper,
    IPagedResponseMapper pagedResponseMapper) : Endpoint<PagedRequestDto, PagedResponseDto<EmailResponseDto>>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiRoutes.Emails.GetEmails);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Produces<PagedResponseDto<EmailResponseDto>>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.Emails.GetEmails;
            s.ExampleRequest = ExampleRequests.Emails.GetEmails;
        });

        Options(x => x.WithTags(SwaggerTags.Emails));
    }

    public override async Task HandleAsync(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        var validator = new PagedRequestDtoValidator<EmailMessage>(emailRepository);
        await validator.ValidateAndThrowAsync(pagedRequestDto, cancellationToken);

        var pagedRequest = mapper.Map<PagedRequest>(pagedRequestDto);

        var pagedResponse = await emailService.GetAllAsync(pagedRequest);

        var pagedResponseDto = pagedResponseMapper.Map<EmailMessage, EmailResponseDto>(pagedResponse);

        await SendAsync(pagedResponseDto, cancellation: cancellationToken);
    }
}