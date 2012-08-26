using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;
using Raven.Client.Document;
using TJ.Extensions;

namespace NBlog.Views
{
    public class RavenViewRepository<T> : IViewRepository<T>, IDisposable
    {
        private DocumentStore _documentStore;
        private IDocumentSession _session;

        public RavenViewRepository(string connectionStringName)
        {
            _documentStore = new DocumentStore()
                                 {
                                     ConnectionStringName = connectionStringName
                                 };
            _documentStore.Initialize();
            _session = _documentStore.OpenSession();
        }

        public void Insert(T postItem)
        {
            _session.Store(postItem);
        }

        public T Find(Func<T, bool> func)
        {
            return _session.Query<T>().SingleOrDefault(func);
        }

        public IEnumerable<T> All(Func<T, bool> predicate = null)
        {
            if(predicate.IsNotNull())
            {
                return _session.Query<T>().Where(predicate);
            }
            return _session.Query<T>().Where(y => true);
        }

        public void Dispose()
        {
            _session.SaveChanges();
        }
    }
}
