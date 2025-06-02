using UserServiceApp.Domain.Common;

namespace UserServiceApp.Domain.UsersAggregate;

public class RefreshToken : BaseEntity
{
    public string Value { get; private set; } = default!;

    public Guid UserId { get; private set; }

    public User? User { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    public RefreshToken(string value, Guid userId, DateTime expiresOnUtc)
    {
        Value = value;
        UserId = userId;
        ExpiresOnUtc = expiresOnUtc;
    }

    public void UpdateValueAndExpiration(string value, DateTime expiresOnUtc)
    {
        Value = value;
        ExpiresOnUtc = expiresOnUtc;
    }

    protected RefreshToken() { }
}
