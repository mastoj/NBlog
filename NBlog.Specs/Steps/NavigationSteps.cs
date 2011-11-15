using System.Collections.Generic;
using System.Net;
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
        private Dictionary<string, string> _pages = NavigationHelper.Pages;
        private Dictionary<string, string> _links = NavigationHelper.Links;

        public NavigationSteps()
        {
        }

        [Given(@"I am on the ""(.*)""")]
        public void GivenIAmOnTheAdminPage(string page)
        {
            var pageUrl = _pages[page];
            WebBrowser.Current.GoTo(Config.Configuration.Host + pageUrl);
        }
        
        [When(@"I navigate to the ""(.*)""")]
        public void WhenINavigateToAPage(string page)
        {
            var pageUrl = _pages[page];
            WebBrowser.Current.GoTo(Config.Configuration.Host + pageUrl);
        }

        [When(@"I am on the ""(.*)""")]
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

        [Then(@"I should be redirected to the ""(.*)""")]
        public void ThenIShouldBeRe_DirectedToThePage(string page)
        {
            WebBrowser.Current.ShouldHave(HttpStatusCode.OK);
            var localPath = WebBrowser.Current.Uri.LocalPath;
            var isMatch = localPath.Equals(_pages[page]);
            if (!isMatch)
            {
                Assert.Fail("Was not redirected to the page {0}", page);
            }
        }
    
        [Then(@"there should be a ""(.*)"" link")]
        public void ThenThereShouldBeALogOffLink(string linkId)
        {
            var link = WebBrowser.Current.Link(Find.ById(_links[linkId]));
            if (link.Exists.IsFalse())
            {
                Assert.Fail("Can't find link {0}", linkId);
            }
        }
    }
}
