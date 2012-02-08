using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.DDD.Infrastructure.Exceptions
{
    public class UnregisteredEventException : Exception
    {
        public UnregisteredEventException()
        {
            
        }

        public UnregisteredEventException(string message) : base(message)
        {
            
        }
    }
}
