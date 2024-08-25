using Core.Constants;
using Core.Enums;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using FluentValidation;
using Presentation.WebApi.Models.UserManagement;

namespace Presentation.WebApi.Endpoints.UserManagement;

public class DeleteUser(IUserService userService,
    IValidator<GetUserRequestDto> validator) : Endpoint<GetUserRequestDto>
{
    public override void Configure()
    {
        Version(1);
        Delete(ApiUrls.UserManagement.DeleteUser);
        Roles(UserRole.Admin.ToString());
        EnableAntiforgery();
    }

    public override async Task HandleAsync(GetUserRequestDto getUserRequestDto, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(getUserRequestDto, cancellationToken);

        var user = HttpContext.Items["User"] as Core.Models.User;

        await userService.DeleteAsync(new UserId(getUserRequestDto.UserId), user!.Username);

        await SendNoContentAsync(cancellation: cancellationToken);
    }
}