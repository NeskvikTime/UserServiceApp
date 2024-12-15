using UserServiceApp.Domain.Common;
using UserServiceApp.Domain.UsersAggregate;

namespace UserServiceApp.Domain;
public class RefreshToken : BaseEntity
{
    public string Token { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public DateTime ExpiresOnUtc { get; private set; }

    protected RefreshToken() { }

    public RefreshToken(string token, Guid userId, DateTime expiresOnUtc)
    {
        Token = token;
        UserId = userId;
        ExpiresOnUtc = expiresOnUtc;
    }
}
