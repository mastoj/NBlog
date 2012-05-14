using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserCreatedEvent : DomainEventBase
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(string userId, string name, string email, Guid aggregateId)
        {
            UserId = userId;
            Name = name;
            Email = email;
            AggregateId = aggregateId;
        }
    }
}