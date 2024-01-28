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

    public string Password { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public bool IsAdmin { get; }

    public User(string userName,
        string fullName,
        string email,
        string mobileNumber,
        string language,
        string culture,
        string password,
        bool isAdmin = false)
    {
        FullName = fullName;
        Username = userName;
        Email = email;
        MobileNumber = mobileNumber;
        Language = language;
        Culture = culture;
        Password = password;
        IsAdmin = isAdmin;
    }

    public bool CheckPasswordAsync(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, PasswordHash);
    }

    public void AssignPasswordAndHash(string password, string hash)
    {
        Password = password;
        PasswordHash = hash;
    }

    protected User() { }
}
