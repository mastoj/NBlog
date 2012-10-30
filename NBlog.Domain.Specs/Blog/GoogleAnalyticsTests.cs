using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NUnit.Framework;
using TJ.CQRS.Messaging;

namespace NBlog.Domain.Specs.Blog
{
    [TestFixture]
    public class When_Enabling_Google_Analytics_With_UAAccount_12345 : BaseCommandTest<EnableGoogleAnalyticsCommand>
    {
        private Guid _aggregateId;
        private string _uaAccount;

        protected override void Given()
        {
            _aggregateId = Guid.NewGuid();
            _uaAccount = "12345";
            ICommand createBlogCommand = new CreateBlogCommand("Hello", "Hello again", Guid.Empty, _aggregateId);
            PreSetCommand(createBlogCommand);
        }

        protected override EnableGoogleAnalyticsCommand When()
        {
            return new EnableGoogleAnalyticsCommand(_uaAccount, _aggregateId);
        }

        [Test]
        public void Then_A_Google_Analytics_Enabled_Event_Was_Raised()
        {
            var events = GetPublishedEvents().OfType<GoogleAnalyticsEnabledEvent>();
            events.Count().Should().Be(1);

        }

        [Test]
        public void And_The_UAAccount_Was_Set_To_12345()
        {
            var events = GetPublishedEvents().OfType<GoogleAnalyticsEnabledEvent>();
            Assert.AreEqual(_uaAccount, events.First().UAAccount);
        }
    }
}
