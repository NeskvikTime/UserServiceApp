namespace UserServiceApp.Contracts.Users;

public record AuthenticationResponse(
    Guid Id,
    string FullName,
    string Email,
    string Token);