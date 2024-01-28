using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.RegisterUser;
public record RegisterUserCommand(
    string UserName,
    string FullName,
    string Email,
    string Password,
    string MobileNumber,
    string Language,
    bool isAdmin = false) : IRequest<AuthenticationResult>;