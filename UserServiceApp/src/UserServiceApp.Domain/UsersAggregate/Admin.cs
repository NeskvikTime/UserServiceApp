using UserServiceApp.Domain.Common;

namespace UserServiceApp.Domain.UsersAggregate;
public class Admin : BaseEntity
{
    public Guid UserId { get; set; }
}