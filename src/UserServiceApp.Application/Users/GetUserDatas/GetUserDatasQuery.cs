using MediatR;
using UserServiceApp.Application.Common.Authorization;
using UserServiceApp.Contracts.Users;

namespace UserServiceApp.Application.Users.GetUserDatas;

[Authorize(Roles = "Admin")]
public record GetUserDataQuery(Guid? UserId) : IRequest<List<UserResponse>>;