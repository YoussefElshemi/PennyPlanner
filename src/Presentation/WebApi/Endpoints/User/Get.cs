using System.Net;
using Core.Constants;
using FastEndpoints;
using Presentation.Constants;
using Presentation.WebApi.Models.User;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.WebApi.Endpoints.User;

public class Get(IMapper mapper) : EndpointWithoutRequest<UserProfileResponseDto>
{
    public override void Configure()
    {
        Version(1);
        Get(ApiUrls.User.Get);
        EnableAntiforgery();

        Description(b => b
            .Produces<UserProfileResponseDto>()
            .Produces((int)HttpStatusCode.Unauthorized)
            .Produces<ProblemDetails>((int)HttpStatusCode.InternalServerError));

        Summary(s =>
        {
            s.Summary = SwaggerSummaries.User.Get;
            s.Description = SwaggerSummaries.User.Get;
        });

        Options(x => x.WithTags(SwaggerTags.User));
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var user = HttpContext.Items["User"] as Core.Models.User;

        var response = mapper.Map<UserProfileResponseDto>(user!);

        await SendAsync(response, cancellation: cancellationToken);
    }
}