using System;
using System.Collections.Generic;
using System.Linq;
using NBlog.Views;

namespace NBlog.Domain.Specs.Stubs
{
    public class ViewRepositoryStub<T> : IViewRepository<T>
    {
        private List<T> _blogViewItems;

        public ViewRepositoryStub()
        {
            _blogViewItems = new List<T>();
        }

        public void Insert(T blogViewItem)
        {
            _blogViewItems.Add(blogViewItem);
        }

        public T Find(Func<T, bool> predicate)
        {
            return _blogViewItems.FirstOrDefault(y => predicate == null || predicate(y));
        }

        public IEnumerable<T> All(Func<T, bool> predicate)
        {
            return _blogViewItems.Where(y => predicate == null || predicate(y));
        }
    }
}