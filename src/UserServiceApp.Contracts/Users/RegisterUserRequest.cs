namespace UserServiceApp.Contracts.Users;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);