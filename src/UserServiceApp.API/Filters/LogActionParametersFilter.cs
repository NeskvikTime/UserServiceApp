using Microsoft.AspNetCore.Mvc.Filters;
using UserServiceApp.Application.Services;

namespace UserServiceApp.API.Filters;

public class LogActionParametersFilter(ILogger<LogActionParametersFilter> _logger, UserPreferences _userPreferences) : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"Executed action {context.ActionDescriptor.DisplayName}");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.ActionDescriptor.DisplayName;
        var parameters = context.ActionArguments;
        var parameterInfo = string.Join(", ", parameters.Select(kv => $"{kv.Key}: {kv.Value}"));

        _logger.LogInformation($"Executing action {actionName} with parameters {parameterInfo}");
    }
}
