using System.Security.Claims;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Endpoints.User;

public class Profile(IUserService userService) : EndpointWithoutRequest<UserProfileDto>
{
    public override void Configure()
    {
        Get("/user/profile");
        EnableAntiforgery();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value!;
        var user = await userService.GetUserAsync(new UserId(Guid.Parse(userId)));

        var response = UserProfileResponseMapper.Map(user!);

        await SendAsync(response, cancellation: cancellationToken);
    }
}