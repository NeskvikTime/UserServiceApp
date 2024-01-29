namespace UserServiceApp.Contracts.Users;
public record UpdateUserRequest(
    string UserName,
    string FullName,
    string Email,
    string MobileNumber,
    bool isAdmin = false,
    string? NewPassword = null);
