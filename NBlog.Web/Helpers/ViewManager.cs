using System;
using System.Collections.Generic;
using NBlog.Views;

namespace NBlog.Web.Helpers
{
    public class ViewManager
    {
        public Dictionary<Type, INBlogView> ViewDictionary { get; private set; }

        public ViewManager(IBlogView blogView, IUserView userView, IPostView postView)
        {
            ViewDictionary = new Dictionary<Type, INBlogView>();
            ViewDictionary.Add(typeof(IBlogView), blogView);
            ViewDictionary.Add(typeof(IUserView), userView);
            ViewDictionary.Add(typeof(IPostView), postView);
        }

        public IEnumerable<INBlogView> GetAllViews()
        {
            return ViewDictionary.Values;
        }

        public TView GetView<TView>() where TView : INBlogView
        {
            return (TView)ViewDictionary[typeof (TView)];
        }
    }
}