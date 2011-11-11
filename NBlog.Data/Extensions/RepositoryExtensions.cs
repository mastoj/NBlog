using NBlog.Domain.Repositories;

namespace NBlog.Domain.Extensions
{
    public static class RepositoryExtensions
    {
        public static T Single<T>(this IRepository<T> repository)
        {
            return repository.Single(y => true);
        }

        public static T First<T>(this IRepository<T> repository)
        {
            return repository.First(y => true);
        }
    }
}
