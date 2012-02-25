using System;

namespace NBlog.Domain.Exceptions
{

    public class PostAlreadyExistsForUrlException : Exception
    {
        public PostAlreadyExistsForUrlException()
        {

        }

        public PostAlreadyExistsForUrlException(string shorUrl)
            : base("The url: " + shorUrl + " already exist.")
        {

        }
    }
}
