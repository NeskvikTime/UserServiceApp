namespace UserServiceApp.Contracts.Users;

public record RegisterUserRequest(
    string UserName,
    string FullName,
    string Email,
    string MobileNumber,
    string Password);