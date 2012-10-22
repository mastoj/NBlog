using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using NBlog.Domain.Commands.Validators;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    [Validator(typeof(CreateBlogCommandValidator))]
    public class CreateBlogCommand : Command
    {
        [Display(Name = "Blog title")]
        public string BlogTitle { get; set; }
        [Display(Name = "Blog title line two")]
        public string Subtitle { get; set; }
        public Guid UserId { get; set; }

        public CreateBlogCommand() : base(Guid.NewGuid())
        {
            
        }
        public CreateBlogCommand(string blogTitle, string subtitle, Guid userId)
            : base(Guid.NewGuid())
        {
            BlogTitle = blogTitle;
            Subtitle = subtitle;
            UserId = userId;
        }
    }
}