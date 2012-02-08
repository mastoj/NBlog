using System;

namespace TJ.DDD.Infrastructure.Exceptions
{
    public class UnregisteredCommandException : Exception
    {
        public UnregisteredCommandException()
        {

        }

        public UnregisteredCommandException(string message)
            : base(message)
        {

        }
    }
}