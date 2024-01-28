namespace UserServiceApp.Contracts.Users;

public record UserResponse(
    Guid Id,
    string Username,
    string FullName,
    string Email,
    string MobileNumber,
    string Language,
    string Culture,
    bool IsAdmin);
