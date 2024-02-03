namespace UserServiceApp.Contracts.Users;
public record UpdateUserRequest(
    string NewUserName,
    string NewFullName,
    string Email,
    string NewMobileNumber,
    bool isAdmin = false,
    string? NewPassword = null);
