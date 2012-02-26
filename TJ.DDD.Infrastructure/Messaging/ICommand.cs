using System;

namespace TJ.DDD.Infrastructure.Messaging
{
    public interface ICommand : IMessage
    {
        Guid AggregateId { get; }
    }
}
