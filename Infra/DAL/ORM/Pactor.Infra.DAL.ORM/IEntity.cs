namespace Pactor.Infra.DAL.ORM
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
        bool Equals(Entity<TId> other);
    }
}