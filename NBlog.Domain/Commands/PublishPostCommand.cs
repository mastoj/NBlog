using System;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    public class PublishPostCommand : Command
    {
        public PublishPostCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}