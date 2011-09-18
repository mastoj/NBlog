using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Norm;
using TJ.Extensions;

namespace NBlog.Data.Mongo
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        private IMongo _provider;
        public IMongoDatabase Database { get { return _provider.Database; } }

        protected Repository(IMongoConfiguration mongoConfiguration = null)
        {
            if (mongoConfiguration.IsNull())
            {
                mongoConfiguration = new MongoConfiguration();
            }
            var connectionString = mongoConfiguration.ConnectionString;
            var options = mongoConfiguration.Options;
            _provider = Norm.Mongo.Create(connectionString, options);
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        public IQueryable<T> All()
        {
            return _provider.GetCollection<T>().AsQueryable();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return All().Where(predicate);
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
            item.Id = Guid.NewGuid();
            _provider.GetCollection<T>().Insert(item);
        }

        public void Delete(T item)
        {
            _provider.GetCollection<T>().Delete(item);
        }

        public void DeleteAll()
        {
            _provider.Database.DropCollection(typeof(T).Name);
        }

        public void Update(T item)
        {
            _provider.GetCollection<T>().Insert(item);
        }
    }
}
