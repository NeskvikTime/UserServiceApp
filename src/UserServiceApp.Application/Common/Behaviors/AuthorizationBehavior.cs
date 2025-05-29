using System.Reflection;
using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Application.Common.Interfaces;
using UserServiceApp.Domain.Exceptions;

namespace UserServiceApp.Application.Common.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserProvider _currentUserProvider = currentUserProvider;

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

        var requiredRoles = authorizationAttributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        bool isAuthorized = currentUser.Roles
            .Any(role => requiredRoles.Any(reqRole => string.Equals(role, reqRole, StringComparison.OrdinalIgnoreCase)));

        if (!isAuthorized)
        {
            throw new AuthorizationException($"User is forbidden from taking this action.");
        }

        return await next();
    }
}
