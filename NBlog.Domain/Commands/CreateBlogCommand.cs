using System;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    public class CreateBlogCommand : Command
    {
        public string BlogTitle { get; set; }
        public string Subtitle { get; set; }
        public Guid UserId { get; set; }

        public CreateBlogCommand(string blogTitle, string subtitle, Guid userId)
            : base(Guid.NewGuid())
        {
            BlogTitle = blogTitle;
            Subtitle = subtitle;
            UserId = userId;
        }
    }
}