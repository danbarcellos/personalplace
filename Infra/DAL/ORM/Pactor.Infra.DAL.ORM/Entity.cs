using System;

namespace Pactor.Infra.DAL.ORM
{
    public abstract class Entity : Entity<Guid>
    {
    }

    public abstract class Entity<TId> : IEntity<TId>
    {
        private int? _transientHashCode;

        public static TId UnsavedId => default(TId);

        public virtual TId Id { get; set; }

        protected virtual int Version { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TId>);
        }

        private static bool IsTransient(Entity<TId> obj)
        {
            return obj != null && Equals(obj.Id, default(TId));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(Entity<TId> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!IsTransient(this) && !IsTransient(other) && Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) || otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (_transientHashCode.HasValue)
                return _transientHashCode.Value;

            if (IsTransient(this))
            {
                _transientHashCode = base.GetHashCode();
                return _transientHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId> lhs, Entity<TId> rhs)
        {
            return Equals(lhs, rhs);
        }

        public static bool operator !=(Entity<TId> lhs, Entity<TId> rhs)
        {
            return !(lhs == rhs);
        }
    }
}