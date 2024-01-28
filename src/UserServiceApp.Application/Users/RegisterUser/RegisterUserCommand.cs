using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.RegisterUser;
public record RegisterUserCommand(
    string UserName,
    string FullName,
    string Email,
    string Password,
    string MobileNumber,
    bool isAdmin = false) : IRequest<AuthenticationResult>;