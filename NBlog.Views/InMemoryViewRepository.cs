using System;
using System.Collections.Generic;
using System.Linq;
using TJ.Extensions;

namespace NBlog.Views
{
    public class InMemoryViewRepository<T> : IViewRepository<T>
    {
        private List<T> _blogViewItems;

        public InMemoryViewRepository()
        {
            _blogViewItems = new List<T>();
        }

        public void Insert(T blogViewItem)
        {
            _blogViewItems.Add(blogViewItem);
        }

        public T Find(Func<T, bool> predicate)
        {
            return _blogViewItems.FirstOrDefault(y => predicate.IsNull() || predicate(y));
        }

        public IEnumerable<T> All(Func<T, bool> predicate)
        {
            return _blogViewItems.Where(y => predicate.IsNull() || predicate(y));
        }
    }
}