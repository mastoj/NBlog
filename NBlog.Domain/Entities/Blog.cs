using System;
using System.Collections.Generic;
using NBlog.Domain.Event;
using TJ.CQRS;

namespace NBlog.Domain.Entities
{
    public class Blog : AggregateRoot
    {
        private List<Guid> _usersIds;
        private string _uaAccount;
        private Dictionary<string, string> _redirectUrls;
        private string _disqusShortName;
        private bool _disqusEnabled;

        public Blog()
        {
            RegisterHandlers();
            _redirectUrls = new Dictionary<string, string>();
        }

        private void RegisterHandlers()
        {
            RegisterEventHandler<BlogCreatedEvent>(BlogCreated);
            RegisterEventHandler<UserAddedToBlogEvent>(UserAddedToBlogEvent);
            RegisterEventHandler<GoogleAnalyticsEnabledEvent>(EnableGoogleAnalytics);
            RegisterEventHandler<RedirectUrlAddedEvent>(Handle);
            RegisterEventHandler<DisqusEnabledEvent>(Handle);
        }

        private void Handle(DisqusEnabledEvent @event)
        {
            _disqusEnabled = true;
            _disqusShortName = @event.ShortName;
        }

        private void Handle(RedirectUrlAddedEvent @event)
        {
            if (_redirectUrls.ContainsKey(@event.OldUrl))
            {
                _redirectUrls[@event.OldUrl] = @event.NewUrl;
            }
            else
            {
                _redirectUrls.Add(@event.OldUrl, @event.NewUrl);
            }
        }

        private void UserAddedToBlogEvent(UserAddedToBlogEvent userAddedToBlogEvent)
        {
            if (_usersIds == null)
            {
                _usersIds = new List<Guid>();
            }
            _usersIds.Add(userAddedToBlogEvent.UserId);
        }

        private void BlogCreated(BlogCreatedEvent blogCreatedEvent)
        {
            AggregateId = blogCreatedEvent.AggregateId;
        }

        private Blog(string title, string subtitle, Guid userId, Guid aggregateId) : this()
        {
            var blogCreatedEvent = new BlogCreatedEvent(title, subtitle, DateTime.Now, aggregateId);
            var userAddedEvent = new UserAddedToBlogEvent(userId, aggregateId);
            Apply(blogCreatedEvent);
            Apply(userAddedEvent);
        }

        public static Blog Create(string title, string subtitle, Guid userId, Guid aggregateId)
        {
            return new Blog(title, subtitle, userId, aggregateId);
        }

        public void EnableGoogleAnalytics(GoogleAnalyticsEnabledEvent googleAnalyticsEnabledEvent)
        {
            _uaAccount = googleAnalyticsEnabledEvent.UAAccount;
        }
        public void EnableGoogleAnalytics(string uaAccount)
        {
            var googleAnalyticsEnabled = new GoogleAnalyticsEnabledEvent(uaAccount, AggregateId);
            Apply(googleAnalyticsEnabled);
        }

        public void AddRedirectUrl(string oldUrl, string newUrl)
        {
            var redirectUrlAddedEvent = new RedirectUrlAddedEvent(AggregateId, oldUrl, newUrl);
            Apply(redirectUrlAddedEvent);
        }

        public void EnableDisqus(string shortName)
        {
            var disqusEnabledEvent = new DisqusEnabledEvent(shortName, AggregateId);
            Apply(disqusEnabledEvent);
        }
    }
}