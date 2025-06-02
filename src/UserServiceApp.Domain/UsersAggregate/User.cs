using UserServiceApp.Domain.Common;
using UserServiceApp.Domain.Common.Interfaces;

namespace UserServiceApp.Domain.UsersAggregate;
public class User : AggregateRoot
{
    public string Username { get; private set; } = default!;

    public string FullName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string MobileNumber { get; private set; } = default!;

    public string Language { get; private set; } = default!;

    public string Culture { get; private set; } = default!;

    public string Password { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public bool IsAdmin { get; private set; }

    public User(string userName,
        string fullName,
        string email,
        string mobileNumber,
        string language,
        string culture,
        bool isAdmin = false,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        FullName = fullName;
        Username = userName;
        Email = email;
        MobileNumber = mobileNumber;
        Language = language;
        Culture = culture;
        // This password here is fake password, in order to distract unauthorized the reader
        Password = "*****************";
        IsAdmin = isAdmin;
    }

    public bool CheckPassword(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, PasswordHash);
    }

    public void AssignPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void UpdateUserData(string userName,
        string newFullName,
        string mobileNumber,
        string newCulture,
        string newLanguage,
        IPasswordHasher passwordHasher,
        bool isAdmin = false,
        string? newPassword = null)
    {
        Username = userName;
        FullName = newFullName;
        MobileNumber = mobileNumber;
        Culture = newCulture;
        Language = newLanguage;
        IsAdmin = isAdmin;

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            Password = newPassword;
            PasswordHash = passwordHasher.HashPassword(newPassword);
        }

        if (isAdmin != IsAdmin)
        {
            IsAdmin = isAdmin;
        }
    }

    protected User() { }
}
