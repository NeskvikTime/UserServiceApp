using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Application.Users.UpdateUserData;

[Authorize(Roles = "Admin")]
public record UpdateUserCommand(
    Guid UserId,
    string UserName,
    string FullName,
    string Email,
    string MobileNumber,
    string NewCulture,
    bool IsAdmin = false,
    string? NewPassword = null) : IRequest<UserResponse>;
