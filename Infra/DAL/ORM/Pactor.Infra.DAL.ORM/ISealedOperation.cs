namespace Pactor.Infra.DAL.ORM
{
    public interface ISealedOperation<out T>
    {
        T Execute(params object[] args);
    }
}