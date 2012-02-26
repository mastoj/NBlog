using System;
using TJ.DDD.Infrastructure.Messaging;

namespace NBlog.Domain.Commands
{
    public class PublishPostCommand : Command
    {
        public PublishPostCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}