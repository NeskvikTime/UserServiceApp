using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ProblemDetails? problemDetails = null;

        switch (context.Exception)
        {
            case ValidationException validationException:

                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation error",
                    Detail = string.Join(" ", validationException.Errors
                        .Select(e => e.ErrorMessage))
                };
                break;

            case KeyNotFoundException _:
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Status = StatusCodes.Status404NotFound,
                    Title = "Not found",
                    Detail = context.Exception.Message
                };
                break;

            case AuthenticationException _:
                problemDetails = new ProblemDetails
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Detail = context.Exception.Message
                };
                break;

            case AuthorizationException _:
                problemDetails = new ProblemDetails
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden",
                    Detail = context.Exception.Message
                };
                break;

            default:
                problemDetails = new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal server error",
                    Detail = context.Exception?.InnerException?.Message ?? context.Exception?.Message
                };
                break;
        }

        context.Result = new JsonResult(problemDetails) { StatusCode = problemDetails.Status };
    }
}
