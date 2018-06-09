namespace PersonalPlace.Domain.Validation.Base
{
    public abstract class DomainConstraint<T>
    {
        public abstract CheckConstraintReturn CheckConstraint(T entity);
    }
}