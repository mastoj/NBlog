using NBlog.Domain.Tests.Entities;

namespace NBlog.Domain.Tests.Builders
{
    public static class Build
    {
        public static PostBuilder APost()
        {
            return new PostBuilder();
        }
    }
}