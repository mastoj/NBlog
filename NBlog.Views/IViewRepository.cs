using System;
using System.Collections.Generic;

namespace NBlog.Views
{
    public interface IViewRepository<T>
    {
        void Insert(T postItem);
        T Find(Func<T, bool> func);
        IEnumerable<T> All(Func<T, bool> func = null);
    }
}