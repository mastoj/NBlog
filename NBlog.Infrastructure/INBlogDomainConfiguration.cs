using TJ.CQRS.Messaging;

namespace NBlog.Infrastructure
{
    public interface INBlogDomainConfiguration
    {
        void ConfigureMessageRouter(IMessageRouter messageRouter);
    }
}