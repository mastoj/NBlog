using System;

namespace TJ.DDD.Infrastructure.Command
{
    public interface ICommand
    {
        Guid AggregateId { get; }
    }
}
