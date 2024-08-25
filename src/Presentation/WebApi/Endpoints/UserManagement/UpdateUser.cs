using System.Net;
using System.Net.Mime;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentation.Constants;
using Presentation.Factories;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Models.User.Validators;
using Presentation.WebApi.Models.UserManagement.Validators;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;
using UpdateUserRequestDto = Presentation.WebApi.Models.User.UpdateUserRequestDto;
using UserManagementUpdateUserRequestDto = Presentation.WebApi.Models.UserManagement.UpdateUserRequestDto;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class UpdateUser(IUserService userService,
    IUserRepository userRepository,
    TimeProvider timeProvider,
    IMapper mapper) : Endpoint<UpdateUserRequestDto, UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Put(ApiUrls.UserManagement.UpdateUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();

        Description(b => b
            .Accepts<UpdateUserRequestDto>(MediaTypeNames.Application.Json)
            .Produces<UserProfileResponseDto>()
            .Produces<ValidationProblemDetails>((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Forbidden)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.UserManagement.UpdateUser;
            s.Description = SwaggerSummaries.UserManagement.UpdateUser;
        });

        Options(x => x.WithTags(SwaggerTags.UserManagement));
    }

    public override async Task HandleAsync(UpdateUserRequestDto updateUserRequestDto, CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        await ValidateAndThrowAsync(updateUserRequestDto, user);

        var updateUserRequest = UpdateUserRequestFactory.Map(user!, updateUserRequestDto) with
        {
            UpdatedBy = user!.Username,
            UpdatedAt = new UpdatedAt(timeProvider.GetUtcNow().UtcDateTime)
        };

        var updatedUser = await userService.UpdateAsync(updateUserRequest);

        var response = mapper.Map<UserProfileResponseDto>(updatedUser);

        await SendAsync(response, cancellation: cancellationToken);
    }

    private async Task ValidateAndThrowAsync(UpdateUserRequestDto updateUserRequestDto, Core.Models.User? user)
    {
        var userManagementUpdateUserRequestDto = mapper.Map<UpdateUserRequestDto, UserManagementUpdateUserRequestDto>(updateUserRequestDto, opt =>
        {
            opt.Items["UserId"] = Route<Guid>("UserId");
        });
        var userManagementValidator = new UserManagementUpdateUserRequestDtoValidator(userRepository);
        await userManagementValidator.ValidateAndThrowAsync(userManagementUpdateUserRequestDto);

        var validator = new UpdateUserRequestDtoValidator(userRepository, user!);
        await validator.ValidateAndThrowAsync(updateUserRequestDto);
    }
}