namespace UserServiceApp.Contracts.Users;
public record UpdateUserRequest(
    string UserName,
    string FullName,
    string Email,
    string MobileNumber,
    string Language,
    string Culture,
    bool isAdmin = false);
