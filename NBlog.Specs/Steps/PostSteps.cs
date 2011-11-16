using System.Linq;
using System.Text.RegularExpressions;
using DeleporterCore.Client;
using NBlog.Domain.Entities;
using NBlog.Specs.Helpers;
using NBlog.Specs.Infrastructure;
using NUnit.Framework;
using TJ.Extensions;
using TechTalk.SpecFlow;
using WatiN.Core;
using Table = TechTalk.SpecFlow.Table;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class PostSteps
    {
        private static readonly string EditPostUrlBase = NavigationHelper.Pages["edit post page"];

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
            var editUrlPattern = EditPostUrlBase + shortUrl;
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
            WebBrowser.Current.GoTo(Config.Configuration.Host + EditPostUrlBase + shortUrl);
        }

        [Then(@"a post with the following content should have been created")]
        public void ThenAPostWithTheFollowingContentShouldHaveBeenCreated(Table table)
        {
            var expectedPost = PostHelper.CreatePostsFromTable(table).First();
            var actualPost = PostRepositoryHelper.GetPosts().First();
            Assert.AreEqual(expectedPost, actualPost, "Posted post is not same as saved post");
        }

        [Given(@"the following posts exists")]
        public void GivenTheFollowingPostsExists(Table table)
        {
            var existingPost = PostHelper.CreatePostsFromTable(table);
            PostHelper.InsertInRepo(existingPost);
        }

        [Then(@"I should see the following list of posts")]
        public void ThenIShouldSeeTheFollowingListOfPosts(Table table)
        {
            var expectedPosts = PostHelper.CreatePostsFromTable(table);
            var actualPost = PostHelper.GetPostsFromRegularListing(WebBrowser.Current);

            ScenarioContext.Current.Pending();
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
