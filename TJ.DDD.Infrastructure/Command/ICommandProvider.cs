using System;
using System.Collections.Generic;

namespace TJ.DDD.Infrastructure.Command
{
    public interface ICommandProvider
    {
        IDictionary<Type, object> GetCommandHandlers();
    }
}