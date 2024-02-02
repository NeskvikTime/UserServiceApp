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
        string passwordHash,
        bool isAdmin = false,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        FullName = fullName;
        Username = userName;
        Email = email;
        MobileNumber = mobileNumber;
        Language = language;
        Culture = culture;
        PasswordHash = passwordHash;
        // This password here is fake password, in order to distract unauthorized the reader
        Password = "*****************";
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

    public void UpdateUserData(string userName,
        string fullName,
        string email,
        string mobileNumber,
        string newCulture,
        string newLanguage,
        IPasswordHasher passwordHasher,
        bool isAdmin = false,
        string? newPassword = null)
    {
        Username = userName;
        FullName = fullName;
        Email = email;
        MobileNumber = mobileNumber;
        Culture = newCulture;
        Language = newLanguage;
        IsAdmin = isAdmin;

        if (!string.IsNullOrEmpty(newPassword))
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
