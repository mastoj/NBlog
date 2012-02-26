using System;

namespace NBlog.Domain.Exceptions
{
    public class DuplicatePostIdException : Exception
    {
        public DuplicatePostIdException() : base("Post already exist for the given id.")
        {
            
        }
    }
}