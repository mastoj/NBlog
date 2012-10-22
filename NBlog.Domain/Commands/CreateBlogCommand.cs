using System;
using FluentValidation.Attributes;
using NBlog.Domain.Commands.Validators;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    [Validator(typeof(CreateBlogCommandValidator))]
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