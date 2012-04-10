using System;

namespace NBlog.Domain.Exceptions
{
    public class PostAlreadyExistsForSlugException : Exception
    {
        public PostAlreadyExistsForSlugException()
        {

        }

        public PostAlreadyExistsForSlugException(string slug)
            : base("The slug: " + slug + " is already in use.")
        {

        }
    }
}
