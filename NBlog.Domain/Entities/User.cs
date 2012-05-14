using System;
using NBlog.Domain.Event;
using TJ.CQRS;

namespace NBlog.Domain.Entities
{
    public class User : AggregateRoot
    {
        private string _userId;
        private string _email;
        private string _name;

        private User(string userId, string name, string email, Guid aggregateId) : this()
        {
            var userCreatedEvent = new UserCreatedEvent(userId, name, email, aggregateId);
            Apply(userCreatedEvent);
        }

        public User()
        {
            RegisterHandlers();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<UserCreatedEvent>(UserCreated);
        }

        private void UserCreated(UserCreatedEvent userCreatedEvent)
        {
            AggregateId = userCreatedEvent.AggregateId;
            _userId = userCreatedEvent.UserId;
            _email = userCreatedEvent.Email;
            _name = userCreatedEvent.Name;
        }

        public static User Create(string userId, string name, string email, Guid aggregateId)
        {
            return new User(userId, name, email, aggregateId);
        }
    }
}
