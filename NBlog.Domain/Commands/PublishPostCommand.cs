using System;
using TJ.DDD.Infrastructure.Command;

namespace NBlog.Domain.Commands
{
    public class PublishPostCommand : ICommand
    {
        private readonly Guid _aggregateId;

        public PublishPostCommand(Guid aggregateId)
        {
            _aggregateId = aggregateId;
        }

        public Guid AggregateId
        {
            get { return _aggregateId; }
        }
    }
}