using System;
using FluentValidation.Attributes;
using NBlog.Domain.Commands.Validators;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    [Validator(typeof(PublishPostCommandValidator))]
    public class PublishPostCommand : Command
    {
        public PublishPostCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }

    [Validator(typeof(PublishPostCommandValidator))]
    public class UnpublishPostCommand : PublishPostCommand
    {
        public UnpublishPostCommand(Guid aggregateId) : base(aggregateId)
        {

        }
    }
}