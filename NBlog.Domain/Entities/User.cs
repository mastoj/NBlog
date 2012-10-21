using System;
using NBlog.Domain.Event;
using TJ.CQRS;

namespace NBlog.Domain.Entities
{
    public class User : AggregateRoot
    {
        private string _authenticationId;
        private string _email;
        private string _name;

        private User(string authenticationId, string name, string email, Guid aggregateId) : this()
        {
            var userCreatedEvent = new UserCreatedEvent(authenticationId, name, email, aggregateId);
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
            _authenticationId = userCreatedEvent.authenticationId;
            _email = userCreatedEvent.Email;
            _name = userCreatedEvent.Name;
        }

        public static User Create(string authenticationId, string name, string email, Guid aggregateId)
        {
            return new User(authenticationId, name, email, aggregateId);
        }
    }
}
