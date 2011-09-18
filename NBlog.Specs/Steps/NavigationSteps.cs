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
        private Dictionary<string, string> _links;

        public NavigationSteps()
        {
            _pages = new Dictionary<string, string>()
                         {
                             {"start page", ""},
                             {"login page", "login"}
                         };
            _links = new Dictionary<string, string>()
                         {
                             {"log off", "logOff"}
                         };
        }

        [When(@"I navigate to the (.*)")]
        public void WhenINavigateToAPage(string page)
        {
            var pageUrl = _pages[page];
            WebBrowser.Current.GoTo(Configuration.Host + pageUrl);
        }

        [When(@"I am on the (.*)")]
        public void WhenIAmOnAPage(string page)
        {
            WhenINavigateToAPage(page);
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

        [Then(@"I should be re-directed to the (.*)")]
        public void ThenIShouldBeRe_DirectedToTheStartPage(string page)
        {
            ScenarioContext.Current.Pending();
        }
    
        [Then(@"there should be a (.*) link")]
        public void ThenThereShouldBeALogOffLink(string link)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
