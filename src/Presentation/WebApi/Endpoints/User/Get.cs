using Core.Constants;
using FastEndpoints;
using Presentation.Mappers;
using Presentation.WebApi.Models.User;

namespace Presentation.WebApi.Endpoints.User;

public class Get : EndpointWithoutRequest<UserProfileResponseDto>
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