using System.Net;
using Core.Constants;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Validators.User;

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
        var user = HttpContext.Items["User"] as Core.Models.User;

        var validator = new UpdateUserRequestDtoValidator(userRepository, user!);
        await validator.ValidateAndThrowAsync(updateUserRequestDto, cancellationToken);

        var updateUserRequest = UpdateUserRequestMapper.Map(user!, updateUserRequestDto) with
        {
            UpdatedBy = user!.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}