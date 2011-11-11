using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using Norm;
using Norm.Collections;
using TJ.Extensions;

namespace NBlog.Domain.Mongo
{
    public abstract class Repository<T> : IRepository<T> where T : IEntity
    {
        private IMongo _provider;
        public IMongoDatabase Database { get { return _provider.Database; } }
        public abstract string CollectionName { get; }

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
            return GetCollection().AsQueryable();
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
            GetCollection().Insert(item);
        }

        public void Delete(T item)
        {
            GetCollection().Delete(item);
        }

        public void DeleteAll()
        {
            _provider.Database.DropCollection(CollectionName);
        }

        public void Update(T item)
        {
            GetCollection().Insert(item);
        }

        private IMongoCollection<T> GetCollection()
        {
            return _provider.GetCollection<T>(CollectionName);
        }
    }
}
