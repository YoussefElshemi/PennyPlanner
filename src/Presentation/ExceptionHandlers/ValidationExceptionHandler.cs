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

        if (Enum.TryParse(validationException.Errors.FirstOrDefault()?.ErrorCode, true, out HttpStatusCode statusCode))
        {
            await WriteErrorHttpResponseAsync(httpContext, validationException, statusCode, cancellationToken);
        }
        else
        {
            await WriteValidationHttpResponseAsync(httpContext, validationException, cancellationToken);
        }

        return true;
    }

    private static Task WriteValidationHttpResponseAsync(HttpContext httpContext, ValidationException exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        return httpContext.Response.WriteAsJsonAsync(
            new ValidationProblemDetails
            {
                Type = exception.GetType().Name,
                Title = HttpStatusCode.BadRequest.ToString(),
                Detail = "Please refer to the errors property for additional details.",
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode,
                Errors = exception.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage)
                        .ToArray())
            }, cancellationToken);
    }

    private static Task WriteErrorHttpResponseAsync(HttpContext httpContext, ValidationException exception, HttpStatusCode statusCode, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        return httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Type = statusCode.ToString(),
                Title = statusCode.ToString(),
                Detail = exception.Errors.FirstOrDefault()?.ErrorMessage,
                Instance = httpContext.Request.Path,
                Status = httpContext.Response.StatusCode
            }, cancellationToken);
    }
}