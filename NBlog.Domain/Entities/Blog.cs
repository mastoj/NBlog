using System;
using NBlog.Domain.Event;
using TJ.CQRS;

namespace NBlog.Domain.Entities
{
    public class Blog : AggregateRoot
    {
        public Blog()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<BlogCreatedEvent>(BlogCreated);
            RegisterEventHandler<UserAddedEvent>(UserAdded);
        }

        private void UserAdded(UserAddedEvent userAddedEvent)
        {

        }

        private void BlogCreated(BlogCreatedEvent blogCreatedEvent)
        {
            AggregateId = blogCreatedEvent.AggregateId;
        }

        private Blog(string title, string subtitle, string adminId, string authorName, string authorEmail, Guid aggregateId) : this()
        {
            var blogCreatedEvent = new BlogCreatedEvent(title, subtitle, DateTime.Now, aggregateId);
            var userAddedEvent = new UserAddedEvent(adminId, authorName, authorEmail, aggregateId);
            Apply(blogCreatedEvent);
            Apply(userAddedEvent);
        }

        public static Blog Create(string title, string subtitle, string adminId, string authorName, string authorEmail, Guid aggregateId)
        {
            return new Blog(title, subtitle, adminId, authorName, authorEmail, aggregateId);
        }
    }
}