using System.Net;
using System.Net.Mime;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Factories;
using Presentation.WebApi.AuthenticatedUser.Models.Requests;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.AuthenticatedUser.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.AuthenticatedUser.Endpoints;

public class Update(
    IUserRepository userRepository,
    IUserService userService,
    TimeProvider timeProvider,
    IMapper mapper) : Endpoint<UpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Put(ApiRoutes.User.Update);
        EnableAntiforgery();

        Description(b => b
            .Accepts<UpdateUserRequestDto>(MediaTypeNames.Application.Json)
            .Produces<UserProfileResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.User.Update;
            s.ExampleRequest = ExampleRequests.User.Update;
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var authenticatedUser = HttpContext.Items["User"] as User;

        var validator = new UpdateUserRequestDtoValidator(userRepository, authenticatedUser!);
        await validator.ValidateAndThrowAsync(updateUserRequestDto, cancellationToken);

        var updateUserRequest = UpdateUserRequestFactory.Create(authenticatedUser!, updateUserRequestDto);
        updateUserRequest = updateUserRequest with
        {
            UpdatedBy = updateUserRequest.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendAsync(response, (int)HttpStatusCode.Created, cancellationToken);
    }
}