using System.Net;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Validators.User;
using Presentation.WebApi.Validators.UserManagement;
using UpdateUserRequestDto = Presentation.WebApi.Models.User.UpdateUserRequestDto;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class UpdateUser(IUserService userService,
    IUserRepository userRepository,
    TimeProvider timeProvider) : Endpoint<UpdateUserRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Put(ApiUrls.UserManagement.UpdateUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        await ValidateAndThrowAsync(updateUserRequestDto, user);

        var updateUserRequest = UpdateUserRequestMapper.Map(user!, updateUserRequestDto) with
        {
            UpdatedBy = user!.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }

    private async Task ValidateAndThrowAsync(UpdateUserRequestDto updateUserRequestDto, Core.Models.User? user)
    {
        var userManagementUpdateUserRequestDto = UpdateUserRequestDtoMapper.Map(Route<Guid>("UserId"), updateUserRequestDto);
        var userManagementValidator = new UserManagementUpdateUserRequestDtoValidator(userRepository);
        await userManagementValidator.ValidateAndThrowAsync(userManagementUpdateUserRequestDto);

        var validator = new UpdateUserRequestDtoValidator(userRepository, user!);
        await validator.ValidateAndThrowAsync(updateUserRequestDto);
    }
}