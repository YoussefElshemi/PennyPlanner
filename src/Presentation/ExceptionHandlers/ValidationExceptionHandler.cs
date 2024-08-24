using System.Net;
using System.Net.Mime;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        await WriteHttpResponseAsync(httpContext, validationException, cancellationToken);

        return true;
    }

    private static Task WriteHttpResponseAsync(HttpContext httpContext, ValidationException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        return httpContext.Response.WriteAsJsonAsync(
            new ValidationProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Bad Request",
                Detail = "Please refer to the errors property for additional details.",
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode,
                Errors = exception.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage)
                    .ToArray())

            }, cancellationToken: cancellationToken);
    }
}