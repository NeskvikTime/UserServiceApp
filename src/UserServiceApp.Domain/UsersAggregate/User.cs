using UserServiceApp.Domain.Common;
using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.Domain.UsersAggregate;
public class User : AggregateRoot
{
    public string Username { get; } = default!;

    public string FullName { get; } = default!;

    public string Email { get; } = default!;

    public string MobileNumber { get; } = default!;

    public string Language { get; } = default!;

    public string Culture { get; } = default!;

    public string Password { get; set; } = default!;

    public string? PasswordHash { get; set; } = default!;

    public Guid? AdminId { get; }

    public User(string userName,
        string fullName,
        string email,
        string mobileNumber,
        string language,
        string culture,
        string password)
    {
        FullName = fullName;
        Username = userName;
        Email = email;
        MobileNumber = mobileNumber;
        Language = language;
        Culture = culture;
        Password = password;
    }

    public virtual Admin? Admin { get; }

    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, PasswordHash);
    }

    protected User() { }
}
