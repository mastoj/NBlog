using System;

namespace TJ.DDD.Infrastructure.Command
{
    public class Command : ICommand
    {
        public Guid AggregateId { get; private set; }

        public Command(Guid aggregateId)
        {
            AggregateId = aggregateId;
        }
    }
}