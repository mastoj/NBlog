using System;
using System.Collections.Generic;
using System.Linq;

namespace NBlog.Views.Tests
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
            return _blogViewItems.FirstOrDefault(predicate);
        }

        public IEnumerable<T> All(Func<T, bool> predicate)
        {
            return _blogViewItems.Where(predicate);
        }
    }
}