using NBlog.Views;

namespace NBlog.Web.Models
{
    public class PostItemViewModel
    {
        public PostItemViewModel(PostItem postItem)
        {
            PostItem = postItem;
        }

        public PostItem PostItem { get; set; }
        public bool IsEditMode { get; set; }
    }
}