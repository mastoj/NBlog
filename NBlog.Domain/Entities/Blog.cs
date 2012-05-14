using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using TJ.CQRS;

namespace NBlog.Domain.Entities
{
    public class Blog : AggregateRoot
    {
        private List<string> _usersIds;

        public Blog()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<BlogCreatedEvent>(BlogCreated);
            RegisterEventHandler<UserAddedToBlogEvent>(UserAddedToBlogEvent);
        }

        private void UserAddedToBlogEvent(UserAddedToBlogEvent UserAddedToBlogEvent)
        {
            if (_usersIds == null)
            {
                _usersIds = new List<string>();
            }
            _usersIds.Add(UserAddedToBlogEvent.UserId);
        }

        private void BlogCreated(BlogCreatedEvent blogCreatedEvent)
        {
            AggregateId = blogCreatedEvent.AggregateId;
        }

        private Blog(string title, string subtitle, string userId, Guid aggregateId) : this()
        {
            var blogCreatedEvent = new BlogCreatedEvent(title, subtitle, DateTime.Now, aggregateId);
            var userAddedEvent = new UserAddedToBlogEvent(userId, aggregateId);
            Apply(blogCreatedEvent);
            Apply(userAddedEvent);
        }

        public static Blog Create(string title, string subtitle, string userId, Guid aggregateId)
        {
            return new Blog(title, subtitle, userId, aggregateId);
        }
    }
}