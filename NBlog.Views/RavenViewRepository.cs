using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using TJ.Extensions;

namespace NBlog.Views
{
    public class RavenViewRepository<T> : IViewRepository<T>
    {
        private DocumentStore _documentStore;

        public RavenViewRepository(string url)
        {
            _documentStore = new DocumentStore()
                                 {
                                     Url = url
                                 };
            _documentStore.Initialize();
        }

        public void Insert(T postItem)
        {
            using(var session = _documentStore.OpenSession())
            {
                session.Store(postItem);
                session.SaveChanges();
            }
        }

        public T Find(Func<T, bool> func)
        {
            using(var session = _documentStore.OpenSession())
            {
                return session.Query<T>().SingleOrDefault(func);
            }
        }

        public IEnumerable<T> All(Func<T, bool> predicate = null)
        {
            using (var session = _documentStore.OpenSession())
            {
                if(predicate.IsNotNull())
                {
                    return session.Query<T>().Where(predicate);
                }
                return session.Query<T>().Where(y => true);
            }
        }
    }
}
