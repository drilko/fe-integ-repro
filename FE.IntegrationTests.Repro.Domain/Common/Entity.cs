namespace FE.IntegrationTests.Repro.Domain.Common;
public abstract class Entity
{
    private int? _requestedHashCode;

    public virtual int Id { get; protected set; }

    public bool IsTransient() => Id == default;

    public override bool Equals(object? obj)
    {
        if (obj is not Entity)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var item = (Entity)obj;

        if (item.IsTransient() || IsTransient())
        {
            return false;
        }

        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = Id.GetHashCode() ^ 31;
            }

            return _requestedHashCode.Value;
        }

        return base.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (Equals(left, null))
        {
            return Equals(right, null);
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right) => !(left == right);
}
