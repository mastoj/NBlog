using System;
using System.Collections.Generic;
using System.Linq;
using TJ.Extensions;

namespace NBlog.Views
{
    public class InMemoryViewRepository<T> : IViewRepository<T>
    {
        private List<T> _items = new List<T>();

        public InMemoryViewRepository()
        {
        }

        public void Insert(T blogViewItem)
        {
            _items.Add(blogViewItem);
        }

        public T Find(Func<T, bool> predicate)
        {
            return _items.FirstOrDefault(y => predicate.IsNull() || predicate(y));
        }

        public IEnumerable<T> All(Func<T, bool> predicate)
        {
            return _items.Where(y => predicate.IsNull() || predicate(y));
        }
    }
}