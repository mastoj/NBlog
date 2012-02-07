using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Domain;

namespace NBlog.Tests
{
    public class InMemoryRepository<T> : IRepository<T>
    {
        private List<T> _items = new List<T>();

        public void Dispose()
        {

        }

        public IQueryable<T> All()
        {
            return _items.AsQueryable();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return All().Where(predicate).AsQueryable();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return All().SingleOrDefault(predicate);
        }

        public T First(Expression<Func<T, bool>> predicate)
        {
            return All().FirstOrDefault(predicate);
        }

        public void Insert(T item)
        {
            _items.Add(item);
        }

        public void Delete(T item)
        {
            _items.Remove(item);
        }

        public void Update(T item)
        {

        }

        public void DeleteAll()
        {
            _items = new List<T>();
        }

        public void Save()
        {

        }        
    }
}