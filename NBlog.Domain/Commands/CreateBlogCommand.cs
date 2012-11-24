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
        public CreateBlogCommand(string blogTitle, string subtitle, Guid userId, Guid aggregateId)
            : base(aggregateId)
        {
            BlogTitle = blogTitle;
            Subtitle = subtitle;
            UserId = userId;
        }
    }

    public class EnableGoogleAnalyticsCommand : Command
    {
        public string UAAccount { get; set; }

        public EnableGoogleAnalyticsCommand(string uaAccount, Guid aggregateId)
            : base(aggregateId)
        {
            UAAccount = uaAccount;
        }
    }

    public class EnableDisqusCommand : Command
    {
        public EnableDisqusCommand(Guid aggregateId, string shortName) : base(aggregateId)
        {
            ShortName = shortName;
        }

        public string ShortName { get; set; }

    }

    public class AddRedirectUrlCommand : Command
    {
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }

        public AddRedirectUrlCommand(Guid aggregateId, string oldUrl, string newUrl) : base(aggregateId)
        {
            OldUrl = oldUrl;
            NewUrl = newUrl;
        }
    }
}