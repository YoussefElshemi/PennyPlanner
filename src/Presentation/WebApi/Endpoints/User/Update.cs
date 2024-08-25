using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Factories;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Models.User.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.User;

public class Update(IUserRepository userRepository,
    IUserService userService,
    TimeProvider timeProvider,
    IMapper mapper) : Endpoint<UpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Put(ApiUrls.User.Update);
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
            s.Description = SwaggerSummaries.User.Update;
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        var validator = new UpdateUserRequestDtoValidator(userRepository, user!);
        await validator.ValidateAndThrowAsync(updateUserRequestDto, cancellationToken);

        var updateUserRequest = UpdateUserRequestFactory.Map(user!, updateUserRequestDto) with
        {
            UpdatedBy = user!.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}