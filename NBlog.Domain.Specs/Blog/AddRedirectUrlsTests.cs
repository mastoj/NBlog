using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NUnit.Framework;

namespace NBlog.Domain.Specs.Blog
{
    [TestFixture]
    public class When_Adding_A_Redirect_Url : BaseCommandTest<AddRedirectUrlCommand>
    {
        private string _oldUrl;
        private string _newUrl;
        private Guid _aggregateId;

        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            PreSetCommand(new CreateBlogCommand("", "", Guid.NewGuid(), _aggregateId));
        }
        
        protected override AddRedirectUrlCommand When()
        {
            _oldUrl = "oldUrl";
            _newUrl = "newUrl";
            return new AddRedirectUrlCommand(_aggregateId, _oldUrl, _newUrl);
        }

        [Test]
        public void Then_Redirect_Url_Added_Event_Is_Raised()
        {
            var raisedEvents = GetPublishedEvents().ToList();
            raisedEvents.Count().Should().Be(1);
            var raisedEvent = raisedEvents[0] as RedirectUrlAddedEvent;
            raisedEvent.Should().BeOfType<RedirectUrlAddedEvent>();
            raisedEvent.AggregateId.Should().Be(_aggregateId);
            raisedEvent.OldUrl.Should().Be(_oldUrl);
            raisedEvent.NewUrl.Should().Be(_newUrl);
        }
    }
}
