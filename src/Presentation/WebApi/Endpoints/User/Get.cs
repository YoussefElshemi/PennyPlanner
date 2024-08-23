using System.Security.Claims;
using Core.Constants;
using Core.Interfaces.Services;
using Core.ValueObjects;
using FastEndpoints;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Endpoints.User;

public class Get(IUserService userService) : EndpointWithoutRequest<UserProfileResponseDto>
{
    public override void Configure()
    {
        Get(ApiUrls.User.Get);
        EnableAntiforgery();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        var response = UserProfileResponseMapper.Map(user!);

        await SendAsync(response, cancellation: cancellationToken);
    }
}