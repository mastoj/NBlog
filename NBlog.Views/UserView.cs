using System;
using System.Collections.Generic;
using System.Linq;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public class UserView : IUserView
    {
        private readonly IViewRepository<UserViewItem> _userViewRepository;

        public UserView(IViewRepository<UserViewItem> userViewRepository)
        {
            _userViewRepository = userViewRepository;
        }

        public IEnumerable<UserViewItem> GetUsers()
        {
            return _userViewRepository.All();
        }

        public void Handle(UserCreatedEvent userAddedEvent)
        {
            var author = new UserViewItem()
                             {
                                 UserId = userAddedEvent.UserId,
                                 UserName = userAddedEvent.Name,
                                 UserEmail = userAddedEvent.Email,
                                 UserAggregateId = userAddedEvent.AggregateId
                             };
            _userViewRepository.Insert(author);
        }

        public UserViewItem GetUser(string identity)
        {
            return _userViewRepository.All().SingleOrDefault(y => y.UserId == identity);
        }
    }

    public class UserViewItem
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid UserAggregateId { get; set; }
    }
}