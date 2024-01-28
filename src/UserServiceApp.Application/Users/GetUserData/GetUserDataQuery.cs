using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.GetUserData;

[Authorize(Roles = "Admin")]
public record GetUserDataQuery(Guid UserId) : IRequest<AuthenticationResult>;