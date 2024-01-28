using MediatR;
using System.Reflection;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Application.Common.Interfaces;

namespace UserServiceApp.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider _currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        if (authorizationAttributes.Count == 0)
        {
            return await next();
        }

        var currentUser = _currentUserProvider.GetCurrentUser();

        var requiredPermissions = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            //return (dynamic)Error.Unauthorized(description: "User is forbidden from taking this action");
        }

        return await next();
    }
}
