using MediatR;
using UserServiceApp.Application.Common.Authorization;

namespace UserServiceApp.Application.Users.DeleteUser;

[Authorize(Roles = "Admin")]
public record DeleteUserCommand(Guid UserId) : IRequest<Unit>;
