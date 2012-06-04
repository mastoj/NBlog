using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Domain.Commands;
using NBlog.Views;

namespace NBlog.Areas.Admin.Models
{
    public class EditPostModel : UpdatePostCommand
    {
        public EditPostModel(PostItem post) : base(post.Title, post.Content, post.Slug, post.Tags, post.Excerpt, Guid.NewGuid())
        {
        }

        public PostItem Post { get; set; }
    }
}
