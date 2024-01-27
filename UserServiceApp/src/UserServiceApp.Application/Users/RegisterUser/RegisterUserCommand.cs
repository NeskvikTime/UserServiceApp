using MediatR;
using UserServiceApp.Contracts.Common;

namespace UserServiceApp.Application.Users.RegisterUser;
public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    bool isAdmin = false) : IRequest<AuthenticationResult>;