using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        [When(@"I am on the post with the ""(.*)"" adress")]
        public void WhenIAmOnThePostWithTheDemopostAdress(string shortUrl)
        {
            WebBrowser.Current.GoTo(Config.Configuration.Host + "/" + shortUrl);
        }

        [Then(@"I should see the post")]
        public void ThenIShouldSeeThePost(Table table)
        {
            var expectedPost = PostHelper.CreatePostsFromTable(table).First();
            var actualPost = PostHelper.GetPostFromPostPage(WebBrowser.Current);
            var tagsAreEqual = expectedPost.PostMetaData.Tags.Comparer(actualPost.PostMetaData.Tags).By(y => y).AreEqual();
            var categoriesAreEqual = expectedPost.PostMetaData.Categories.Comparer(actualPost.PostMetaData.Categories).By(y => y).AreEqual();
            Assert.AreEqual(expectedPost.PostMetaData.Title, actualPost.PostMetaData.Title, "Title is not the same");
            Assert.AreEqual(expectedPost.PublishedPost.Content, actualPost.PublishedPost.Content, "Content is not the same");
            Assert.AreEqual(expectedPost.PublishedPost.PublishDate, actualPost.PublishedPost.PublishDate, "Publish date is not the same");
            Assert.IsTrue(tagsAreEqual, "Tags are not the same");
            Assert.IsTrue(categoriesAreEqual, "Categories are not the same");
        }

        [Then(@"a post with the following meta data should have been created")]
        public void ThenAPostWithTheFollowingMetaDataShouldHaveBeenCreated(Table table)
        {
            var expectedPost = PostHelper.CreateMetaDataFromTable(table).First();
            var actualPost = PostRepositoryHelper.GetPosts().First().PostMetaData;
            Assert.AreEqual(expectedPost, actualPost, "Posted post is not same as saved post");
        }

        [Given(@"the following posts exists")]
        public void GivenTheFollowingPostsExists(Table table)
        {
            var existingPost = PostHelper.CreatePostsFromTable(table);
            PostHelper.InsertInRepo(existingPost);
        }

        [Then(@"I should see the following list of posts in this order")]
        public void ThenIShouldSeeTheFollowingListOfPosts(Table table)
        {
            var expectedPosts = PostHelper.CreatePostsFromTable(table);
            var actualPost = PostHelper.GetPostsFromRegularListing(WebBrowser.Current);

            var areEqual = expectedPosts.Comparer(actualPost).By(y => y.PostMetaData.ShortUrl).And(y => y.PostMetaData.Excerpt).AreEqual();
            Assert.IsTrue(areEqual);
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
