using System.Net;
using Core.Constants;
using Core.Interfaces.Services;
using FastEndpoints;
using FluentValidation;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;
using Presentation.WebApi.Validators.User;

namespace Presentation.WebApi.Endpoints.User;

public class ChangePassword(IAuthenticationService authenticationService) : Endpoint<ChangePasswordRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Patch(ApiUrls.User.ChangePassword);
        EnableAntiforgery();
    }

    public override async Task HandleAsync(ChangePasswordRequestDto changePasswordRequestDto, CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        var validator = new ChangePasswordRequestDtoValidator(authenticationService, user!);
        await validator.ValidateAndThrowAsync(changePasswordRequestDto, cancellationToken);

        var changePasswordRequest = ChangePasswordRequestMapper.Map(changePasswordRequestDto);

        var updatedUser = await authenticationService.ChangePasswordAsync(user!, changePasswordRequest.Password);

        var response = UserProfileResponseMapper.Map(updatedUser);

        await SendAsync(response, statusCode: (int)HttpStatusCode.Created, cancellation: cancellationToken);
    }
}