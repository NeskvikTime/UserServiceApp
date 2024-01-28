namespace UserServiceApp.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot() { }
}