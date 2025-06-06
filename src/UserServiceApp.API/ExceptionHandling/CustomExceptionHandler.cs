using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.API.ExceptionHandling;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails = exception switch
        {
            ValidationException validationException => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation error",
                Detail = string.Join(" ", validationException.Errors.Select(e => e.ErrorMessage))
            },
            NotFoundException => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Detail = exception.Message
            },
            AuthenticationException => new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                Status = StatusCodes.Status403Forbidden,
                Title = "Unauthorized",
                Detail = exception.Message
            },
            AuthorizationException => new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                Status = StatusCodes.Status401Unauthorized,
                Title = "Forbidden",
                Detail = exception.Message
            },
            ArgumentException => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad request",
                Detail = exception.Message
            },
            _ => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error",
                Detail = $"{exception.InnerException?.Message} {exception.Message}"
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }
}
