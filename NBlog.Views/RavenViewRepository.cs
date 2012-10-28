using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using TJ.Extensions;

namespace NBlog.Views
{
    public class RavenViewRepository<T> : IViewRepository<T>, IDisposable
    {
        private DocumentStore _documentStore;
        private IDocumentSession _session;
        private static bool _hasInitializedIndexes = false;
        private List<T> _foundInstances;

        public RavenViewRepository(string connectionStringName)
        {
            _documentStore = new DocumentStore()
                                 {
                                     ConnectionStringName = connectionStringName
                                 };
            _documentStore.Initialize();
            CreateIndexes();
            _session = _documentStore.OpenSession();
            _foundInstances = new List<T>();
        }

        private void CreateIndexes()
        {
            if(_hasInitializedIndexes.IsFalse())
            {
                IndexCreation.CreateIndexes(this.GetType().Assembly, _documentStore);
                _hasInitializedIndexes = true;
            }
        }

        public void Insert(T postItem)
        {
            _session.Store(postItem);
        }

        public T Find(Func<T, bool> func)
        {
            var instance = _foundInstances.SingleOrDefault(func);
            if (instance == null)
            {
                instance = _session.Query<T>().SingleOrDefault(func);
                _foundInstances.Add(instance);
            }
            return instance;
        }

        public IEnumerable<T> All(Func<T, bool> predicate = null)
        {
            if(predicate.IsNotNull())
            {
                return _session.Query<T>().Where(predicate);
            }
            return _session.Query<T>().Where(y => true);
        }

        public void Clear(string indexName)
        {
            _session.Advanced.DatabaseCommands.DeleteByIndex(indexName, new IndexQuery());
        }

        public void Dispose()
        {
            _session.SaveChanges();
        }
    }
}
