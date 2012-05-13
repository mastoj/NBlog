using System;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    public class CreateUserCommand : Command
    {
        public string OpenId { get; set; }
        public string FriendlyName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public CreateUserCommand() : base(Guid.NewGuid())
        {
            
        }
    }

    public class CreateBlogCommand : Command
    {
        public string BlogTitle { get; set; }
        public string Subtitle { get; set; }
        public string AdminId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }

        public CreateBlogCommand(string blogTitle, string subtitle, string adminId, string authorName, string authorEmail)
            : base(Guid.NewGuid())
        {
            BlogTitle = blogTitle;
            Subtitle = subtitle;
            AdminId = adminId;
            AuthorName = authorName;
            AuthorEmail = authorEmail;
        }
    }
}