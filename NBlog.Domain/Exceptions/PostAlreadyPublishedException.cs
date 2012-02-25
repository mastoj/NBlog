using System;

namespace NBlog.Domain.Exceptions
{
    public class PostAlreadyPublishedException : Exception
    {
        public PostAlreadyPublishedException()
            : base("The post has already been published.")
        {

        }
    }
}