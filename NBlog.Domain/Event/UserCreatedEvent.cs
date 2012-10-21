using System;
using TJ.CQRS.Event;

namespace NBlog.Domain.Event
{
    public class UserCreatedEvent : DomainEventBase
    {
        public string authenticationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UserCreatedEvent(string authenticationId, string name, string email, Guid aggregateId)
        {
            this.authenticationId = authenticationId;
            Name = name;
            Email = email;
            AggregateId = aggregateId;
        }
    }
}