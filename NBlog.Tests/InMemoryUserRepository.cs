using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NBlog.Data;

namespace NBlog.Tests
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static List<User> _users = new List<User>();

        public void Dispose()
        {

        }

        public IQueryable<User> All()
        {
            return _users.AsQueryable();
        }

        public IQueryable<User> FindAll(Expression<Func<User, bool>> predicate)
        {
            return All().Where(predicate).AsQueryable();
        }

        public User Single(Expression<Func<User, bool>> predicate)
        {
            return All().SingleOrDefault(predicate);
        }

        public User First(Expression<Func<User, bool>> predicate)
        {
            return All().FirstOrDefault(predicate);
        }

        public void Insert(User item)
        {
            item.Id = Guid.NewGuid();
            _users.Add(item);
        }

        public void Delete(User item)
        {
            _users.Remove(item);
        }

        public void Update(User item)
        {

        }

        public void DeleteAll()
        {
            _users = new List<User>();
        }

        public void Save()
        {

        }
    }
}