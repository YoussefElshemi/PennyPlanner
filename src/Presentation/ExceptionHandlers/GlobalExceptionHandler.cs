using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await WriteHttpResponseAsync(httpContext, exception, cancellationToken);

        return true;
    }

    private static Task WriteHttpResponseAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        return httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = httpContext.Response.StatusCode,
                Instance = httpContext.Request.Path
            }, cancellationToken);
    }
}