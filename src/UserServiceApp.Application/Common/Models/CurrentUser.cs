namespace UserServiceApp.Application.Common.Models;

public record CurrentUser(
    Guid Id,
    IReadOnlyList<string> Roles);