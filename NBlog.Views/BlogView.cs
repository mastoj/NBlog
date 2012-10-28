using System;
using System.Collections.Generic;
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

        public void ResetView()
        {
            _blogViewRepository.Clear("BlogViewIndex");
        }
    }

    public interface IBlogView : INBlogView
    {
        IEnumerable<BlogViewItem> GetBlogs();
        void Handle(BlogCreatedEvent createdEvent);
    }

    public class BlogViewItem
    {
        public Guid BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string SubTitle { get; set; }
    }
}