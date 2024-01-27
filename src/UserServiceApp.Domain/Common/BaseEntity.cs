namespace UserServiceApp.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return ((BaseEntity)obj).Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    protected BaseEntity() { }
}