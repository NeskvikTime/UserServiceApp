using Microsoft.AspNetCore.Mvc.Filters;

namespace UserServiceApp.API.Filters;

public class LogActionParametersFilter : IActionFilter
{
    private readonly ILogger _logger;

    public LogActionParametersFilter(ILogger<LogActionParametersFilter> logger)
    {
        _logger = logger;
    }

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
