using System.Net;
using System.Security.Claims;
using Core.Constants;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Validators;

namespace Presentation.WebApi.Endpoints.User;

public class ChangePassword(IUserService userService) : Endpoint<ChangePasswordRequestDto>
{
    public override void Configure()
    {
        Patch(ApiUrls.User.ChangePassword);
        EnableAntiforgery();
    }

    public override async Task HandleAsync(ChangePasswordRequestDto changePasswordRequestDto, CancellationToken cancellationToken)
    {
        var validator = new ChangePasswordRequestDtoValidator();
        await validator.ValidateAndThrowAsync(changePasswordRequestDto, cancellationToken);

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        var user = await userService.GetAsync(new UserId(Guid.Parse(userId)));

        var changePasswordRequest = ChangePasswordRequestMapper.Map(changePasswordRequestDto);

        var updatedUser = await userService.ChangePasswordAsync(user, changePasswordRequest.Password);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}