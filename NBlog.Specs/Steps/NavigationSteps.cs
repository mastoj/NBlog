using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TJ.Extensions;
using WatiN.Core;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class NavigationSteps
    {
        private Dictionary<string, string> _pages;

        public NavigationSteps()
        {
            _pages = new Dictionary<string, string>()
                         {
                             {"start page", ""}
                         };
        }

        [When(@"I navigatae to the (.*)")]
        public void WhenINavigataeToTheStartPage(string page)
        {
            WebBrowser.Current.GoTo(Configuration.Host);
        }

        [Then(@"it should have a title")]
        public void AndItShouldHaveATitle()
        {
            Assert.IsFalse(WebBrowser.Current.Title.IsNullOrEmpty());
        }

        [Then(@"I should get a successful response")]
        public void ThenItShouldHaveAListOfRecentPosts()
        {
            WebBrowser.Current.ShouldHave(HttpStatusCode.OK);
        }
    }
}
