namespace TJ.DDD.Infrastructure
{
    public interface IUnitOfWork
    {
        void Rollback();
        void Commit();
    }
}