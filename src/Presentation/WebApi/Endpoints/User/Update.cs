using System.Net;
using System.Security.Claims;
using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.User;

public class Update(IUserRepository userRepository,
    IUserService userService,
    TimeProvider timeProvider) : Endpoint<UpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Put(ApiUrls.User.Update);
        EnableAntiforgery();
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserRequestDtoValidator(userRepository);
        await validator.ValidateAndThrowAsync(updateUserRequestDto, cancellationToken);

        var user = HttpContext.Items["User"] as Core.Models.User;

        var updateUserRequest = UpdateUserRequestMapper.Map(user!, updateUserRequestDto) with
        {
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().DateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}