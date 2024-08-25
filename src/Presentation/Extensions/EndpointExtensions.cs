using System.Net;
using FastEndpoints;

namespace Presentation.Extensions;

public static class EndpointExtensions
{
    public static Task SendAccepted(this IEndpoint ep, CancellationToken ct = default)
    {
        ep.HttpContext.MarkResponseStart();
        ep.HttpContext.Response.StatusCode = (int)HttpStatusCode.Accepted;

        return ep.HttpContext.Response.StartAsync(ct);
    }
}