using System;
using System.Linq;
using System.Linq.Expressions;

namespace NBlog.Domain.Repositories
{
    public interface IRepository<T> : IDisposable
    {
        IQueryable<T> All();
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);
        T Single(Expression<Func<T, bool>> predicate);
        T First(Expression<Func<T, bool>> predicate);
        void Insert(T item);
        void Delete(T item);
        void Update(T item);
        void DeleteAll();
    }
}