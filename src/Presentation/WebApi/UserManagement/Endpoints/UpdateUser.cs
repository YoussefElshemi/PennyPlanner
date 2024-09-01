using System.Net;
using System.Net.Mime;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Factories;
using Presentation.WebApi.AuthenticatedUser.Models.Responses;
using Presentation.WebApi.AuthenticatedUser.Validators;
using Presentation.WebApi.UserManagement.Models.Requests;
using Presentation.WebApi.UserManagement.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Presentation.WebApi.UserManagement.Endpoints;

public class UpdateUser(
    IUserService userService,
    IUserRepository userRepository,
    TimeProvider timeProvider,
    IMapper mapper) : Endpoint<UserManagementUpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Put(ApiRoutes.UserManagement.UpdateUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<UserManagementUpdateUserRequestDto>(MediaTypeNames.Application.Json)
            .Produces<UserProfileResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.UpdateUser;
            s.ExampleRequest = ExampleRequests.UserManagement.UserManagementUpdateUser;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(UserManagementUpdateUserRequestDto userManagementUpdateUserRequestDto, CancellationToken cancellationToken)
    {
        var authenticatedUser = HttpContext.Items["User"] as User;

        var user = await ValidateAndThrowAsync(userManagementUpdateUserRequestDto, authenticatedUser!);

        var updateUserRequest = UserManagementUpdateUserRequestFactory.Create(user, userManagementUpdateUserRequestDto);
        updateUserRequest = updateUserRequest with
        {
            UpdatedBy = user.UserId == authenticatedUser!.UserId ? updateUserRequest.Username : authenticatedUser.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendAsync(response, cancellation: cancellationToken);
    }

    private async Task<User> ValidateAndThrowAsync(UserManagementUpdateUserRequestDto userManagementUpdateUserRequestDto, User authenticatedUser)
    {
        var userManagementValidator = new UserManagementUpdateUserRequestDtoValidator(userRepository, authenticatedUser);
        await userManagementValidator.ValidateAndThrowAsync(userManagementUpdateUserRequestDto);

        var user = await userService.GetAsync(new UserId(userManagementUpdateUserRequestDto.UserId));

        var validator = new UpdateUserRequestDtoValidator(userRepository, user);
        await validator.ValidateAndThrowAsync(userManagementUpdateUserRequestDto);

        return user;
    }
}