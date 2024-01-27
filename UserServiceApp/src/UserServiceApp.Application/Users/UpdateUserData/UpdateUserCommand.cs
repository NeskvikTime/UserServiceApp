using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Application.Users.UpdateUserData;

[Authorize(Roles = "Admin")]
public record UpdateUserCommand(
    Guid userId,
    string UserName,
    string FullName,
    string Email,
    string MobileNumber,
    string Language,
    string Culture,
    bool isAdmin = false) : IRequest<User>;
