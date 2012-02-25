using System;

namespace NBlog.Domain.Exceptions
{
    public class PostDoesNotExistException : Exception
    {
        public PostDoesNotExistException()
            : base("The post you are trying to update does not exist")
        {

        }
    }
}