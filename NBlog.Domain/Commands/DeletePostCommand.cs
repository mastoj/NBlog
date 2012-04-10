using System;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Commands
{
    public class DeletePostCommand : Command
    {
        public DeletePostCommand(Guid aggregateId) : base(aggregateId)
        {
        }
    }
}