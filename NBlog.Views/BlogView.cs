using System;
using System.Collections.Generic;
using System.Linq;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public class BlogView : IBlogView
    {
        private readonly IViewRepository<BlogViewItem> _blogViewRepository;

        public BlogView(IViewRepository<BlogViewItem> blogViewRepository)
        {
            _blogViewRepository = blogViewRepository;
        }

        public IEnumerable<BlogViewItem> GetBlogs()
        {
            return _blogViewRepository.All(y => true);
        }

        public void Handle(BlogCreatedEvent createdEvent)
        {
            var blogViewItem = new BlogViewItem()
                                   {
                                       BlogId = createdEvent.AggregateId,
                                       BlogTitle = createdEvent.BlogTitle,
                                       SubTitle = createdEvent.SubTitle
                                   };
            _blogViewRepository.Insert(blogViewItem);
            _blogViewRepository.CommitChanges();
        }

        public void Handle(RedirectUrlAddedEvent redirectUrlAddedEvent)
        {
            var blog = GetBlogs().Single(y => y.BlogId == redirectUrlAddedEvent.AggregateId);
            blog.AddRedirectUrl(redirectUrlAddedEvent.OldUrl, redirectUrlAddedEvent.NewUrl);
            _blogViewRepository.CommitChanges();
        }

        public void Handle(GoogleAnalyticsEnabledEvent googleAnalyticsEnabledEvent)
        {
            var blogViewItem = _blogViewRepository.Find(y => y.BlogId == googleAnalyticsEnabledEvent.AggregateId);
            blogViewItem.UAAccount = googleAnalyticsEnabledEvent.UAAccount;
            blogViewItem.GoogleAnalyticsEnabled = true;
            _blogViewRepository.CommitChanges();
        }

        public void Handle(DisqusEnabledEvent disqusEnabledEvent)
        {
            var blogViewItem = _blogViewRepository.Find(y => y.BlogId == disqusEnabledEvent.AggregateId);
            blogViewItem.DisqusShortName = disqusEnabledEvent.ShortName;
            blogViewItem.DisqusEnabled = true;
            _blogViewRepository.CommitChanges();
        }

        public void ResetView()
        {
            _blogViewRepository.Clear("BlogViewIndex");
        }
    }

    public interface IBlogView : INBlogView
    {
        IEnumerable<BlogViewItem> GetBlogs();
        void Handle(BlogCreatedEvent createdEvent);
        void Handle(RedirectUrlAddedEvent redirectUrlAddedEvent);
    }

    public class BlogViewItem
    {
        private Dictionary<string, string> _redirectUrls;
        public Dictionary<string, string> RedirectUrls { get { return _redirectUrls; } set { _redirectUrls = value; } } 
        public Guid BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string SubTitle { get; set; }

        public bool GoogleAnalyticsEnabled { get; set; }

        public string UAAccount { get; set; }

        public string DisqusShortName { get; set; }

        public bool DisqusEnabled { get; set; }

        public void AddRedirectUrl(string oldUrl, string newUrl)
        {
            if (_redirectUrls == null) _redirectUrls = new Dictionary<string, string>();
            _redirectUrls.Add(oldUrl, newUrl);
        }
    }
}