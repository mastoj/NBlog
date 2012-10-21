using System;
using System.Collections.Generic;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public interface IUserView
    {
        IEnumerable<UserViewItem> GetUsers();
        void Handle(UserCreatedEvent userAddedEvent);
        UserViewItem GetUserByAuthenticationId(string authenticationId);
    }
}