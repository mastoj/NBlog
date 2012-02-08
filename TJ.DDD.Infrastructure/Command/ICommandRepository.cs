using System;
using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Command
{
    public interface ICommandRepository
    {
        IDictionary<Type, object> GetCommandHandlers();
    }
}