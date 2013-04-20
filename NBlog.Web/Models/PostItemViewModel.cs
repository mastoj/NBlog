using System.Collections.Generic;
using NBlog.Domain.Commands;
using NBlog.Views;

namespace NBlog.Web.Models
{
    public class PostItemViewModel
    {
        public PostItemViewModel(PostItem postItem)
        {
            PostItem = postItem;
            UpdatePostCommand = CreateUpdatePostCommand(postItem);
        }

        public PostItem PostItem { get; set; }
        public bool IsAdminMode { get; set; }
        public UpdatePostCommand UpdatePostCommand { get; set; }

        private UpdatePostCommand CreateUpdatePostCommand(PostItem postItem)
        {
            var updatePostCommand = new UpdatePostCommand(postItem.Title, postItem.Content, postItem.Slug, postItem.Tags,
                                                          postItem.Excerpt, postItem.AggregateId);
            return updatePostCommand;
        }
    }

    public class PostIndexViewModel
    {
        public string BlogTitle { get; set; }
        public IEnumerable<PostItem> Posts { get; set; }
    }
}