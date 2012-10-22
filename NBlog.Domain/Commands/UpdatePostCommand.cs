using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using NBlog.Domain.Commands.Validators;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    [Validator(typeof(UpdatePostCommandValidator))]
    public class UpdatePostCommand : Command
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public List<string> Tags { get; set; }
        public string Excerpt { get; set; }

        public UpdatePostCommand(string title, string content, string slug, List<string> tags, string excerpt, Guid aggregateId) : base(aggregateId)
        {
            Title = title;
            Content = content;
            Slug = slug;
            Tags = tags;
            Excerpt = excerpt;
        }

        public UpdatePostCommand() : base(Guid.Empty)
        {
        }
    }
}