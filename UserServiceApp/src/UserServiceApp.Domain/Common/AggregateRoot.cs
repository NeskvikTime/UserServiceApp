namespace UserServiceApp.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected readonly List<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }

    protected AggregateRoot() { }
}