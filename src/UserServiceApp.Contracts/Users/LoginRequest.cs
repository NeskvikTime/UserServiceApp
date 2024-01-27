namespace UserServiceApp.Contracts.Users;

public record LoginRequest(
    string Email,
    string Password);