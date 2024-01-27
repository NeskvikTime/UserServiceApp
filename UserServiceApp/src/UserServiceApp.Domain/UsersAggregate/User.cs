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

    private readonly string _passwordHash = null!;

    public Guid? AdminId { get; }

    public virtual Admin? Admin { get; }

    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }

    protected User() { }
}
