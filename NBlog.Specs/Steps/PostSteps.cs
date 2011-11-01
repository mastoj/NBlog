using System.Text.RegularExpressions;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TJ.Extensions;
using TechTalk.SpecFlow;
using WatiN.Core;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class PostSteps
    {
        [BeforeScenario("NoPosts")]
        public void DeletePosts()
        {
            PostHelper.DeletePosts();
        }

        [Then(@"I should find a list of posts with one entry")]
        public void ThenIShouldFindAListWithOneEntry()
        {
            var list = WebBrowser.Current.Table(Find.ByClass("posts"));
            if (list.Exists.IsFalse())
            {
                Assert.Fail("Can't find post list");
            }
        }

        [Then(@"it contains the string ""(.*)""")]
        public void ThenItContainsTheStringDemoTitle(string stringToLookFor)
        {
            var stringExist = WebBrowser.Current.ContainsText(stringToLookFor);
            if (stringExist.IsFalse())
            {
                Assert.Fail("Couldn't find the text {0}", stringToLookFor);
            }
        }

        [Then(@"it contains an edit post link to (.*)")]
        public void ThenItContainsAnEditPostLinkToDemopost(string shortUrl)
        {
            var editUrlPattern = "Edit/" + shortUrl;
            CheckIfLinkExists(editUrlPattern);
        }

        [Then(@"it contains an delete post link to (.*)")]
        public void ThenItContainsAnDeletePostLinkToDemopost(string shortUrl)
        {
            var deleteUrlPattern = "Delete/" + shortUrl;
            CheckIfLinkExists(deleteUrlPattern);
        }

        [When(@"I navigate to edit of (.*)")]
        public void WhenINavigateToEditOfDemopost(string shortUrl)
        {
            WebBrowser.Current.GoTo(Config.Configuration.Host + NavigationHelper.Pages["login page"] + shortUrl);
        }

        private void CheckIfLinkExists(string urlPattern)
        {
            var regex = new Regex(urlPattern);
            var editPostLink = WebBrowser.Current.Link(Find.By("href", regex));
            if (editPostLink.Exists.IsFalse())
            {
                Assert.Fail("Can't find link " + urlPattern);
            }
        }
    }
}
