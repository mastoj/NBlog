using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeleporterCore.Client;
using NBlog.Domain.Entities;
using NBlog.Domain.Mongo.Repositories;
using NBlog.Domain.Repositories;
using NBlog.Specs.Config;
using NBlog.Specs.Infrastructure;
using NBlog.Tests.Areas.Admin.Controllers;
using NUnit.Framework;
using TJ.Extensions;
using WatiN.Core;
using List = NUnit.Framework.List;
using Table = TechTalk.SpecFlow.Table;
using TableRow = TechTalk.SpecFlow.TableRow;

namespace NBlog.Specs.Helpers
{
    public static class PostHelper
    {
        public static void DeletePosts()
        {
            Deleporter.Run(() =>
            {
                IPostRepository inMemoryPostRepository = new InMemoryPostRepository();
                DeleporterMvcUtils.TemporarilyReplaceBinding(inMemoryPostRepository);
            });
        }

        public static void InsertInRepo(IEnumerable<Post> existingPost)
        {
            var postsAsList = existingPost.ToList();
            Deleporter.Run(() =>
            {
                IPostRepository inMemoryPostRepository = new InMemoryPostRepository();
                foreach (var post in postsAsList)
                {
                    post.Id = Guid.NewGuid();
                    inMemoryPostRepository.Insert(post);
                }
                DeleporterMvcUtils.TemporarilyReplaceBinding(inMemoryPostRepository);
            });
        }

        public static IEnumerable<Post> CreatePostsFromTable(Table table)
        {
            Func<Post> builderFunc = () => new Post(null);
            var posts = CreateSomethingFromTable(table, configurePostDictionary, builderFunc);
            return posts;
        }

        private static IEnumerable<T> CreateSomethingFromTable<T>(Table table, Dictionary<string, Action<T, string>> actionDictionary, Func<T> builderFunc)
        {
            var items = new List<T>();
            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow(row, actionDictionary, builderFunc);
                items.Add(item);
            }
            return items;
        }

        private static Dictionary<string, Action<Post, string>> configurePostDictionary =
            new Dictionary<string, Action<Post, string>>
                {
                    {"Title", (y, x) => y.PostMetaData.Title = x},
                    {"ShortUrl", (y, x) => y.PostMetaData.ShortUrl = x},
                    {"Content", (y, x) => y.PublishedPost.Content = x},
                    {"PublishDate", (y, x) => y.PublishedPost.PublishDate = ParseDate(x)},
                    {"Tags", (y, x) => y.PostMetaData.Tags = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Categories", (y, x) => y.PostMetaData.Categories = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Excerpt", (y, x) => y.PostMetaData.Excerpt = x}
                };

        private static DateTime ParseDate(string wantedDate)
        {
            if (wantedDate.IsNullOrEmpty())
            {
                return DateTime.MinValue;
            }
            if (wantedDate.ToLower().StartsWith("today"))
            {
                var absoluteDayDiff = int.Parse(wantedDate.Substring(6));
                var dayDiffMultiplier = wantedDate[5] == '+' ? 1 : -1;
                var dayDiff = absoluteDayDiff*dayDiffMultiplier;
                return DateTime.Now.Date.AddDays(dayDiff);
            }
            return DateTime.ParseExact(wantedDate, "yyyy-MM-dd", null);
        }

        private static T CreateItemFromRow<T>(TableRow row, Dictionary<string, Action<T, string>> actionDictionary, Func<T> builderFunc)
        {
            var item = new T();
            foreach (var key in row.Keys)
            {
                var value = row[key];
                if (actionDictionary.ContainsKey(key).IsTrue())
                {
                    actionDictionary[key](item, value);
                }
                else
                {
                    Assert.Fail("Missing key: {0}", key);
                }
            }
            return item;
        }

        public static IEnumerable<Post> GetPostsFromRegularListing(ObservableBrowser browser)
        {
            var postEntryContainer = browser.Div(Find.ByClass(y => y.Contains("posts")));
            var postEntries = postEntryContainer.Divs.Where(y => y.ClassName.Contains("post")).ToList();
            var posts = new List<Post>();
            foreach (var postEntry in postEntries)
            {
                var post = CreatePostFromRegularListingEntry(postEntry);
                posts.Add(post);
            }
            return posts;
        }

        private static Post CreatePostFromRegularListingEntry(Div div)
        {
            var postTitleLink = div.Link(Find.ByClass(y => y.Contains("post-link")));;
            var excerptSpan = div.Span(Find.ByClass(y => y.Contains("excerpt")));
            var post = new Post
                           {
                               PostMetaData = new PostMetaData
                                                  {
                                                      ShortUrl = new Uri(postTitleLink.Url).Segments.LastOrDefault(),
                                                      Title = postTitleLink.InnerHtml,
                                                      Excerpt = excerptSpan.InnerHtml
                                                  }
                           };
            return post;
        }

        public static Post GetPostFromPostPage(ObservableBrowser browser)
        {
            var post = new Post
                           {
                               PostMetaData = new PostMetaData
                                                  {
                                                      Tags =
                                                          browser.List(Find.ById("Tags")).ListItems.SelectMany(
                                                              y => y.Links.Select(x => x.Text.Trim())).ToList(),
                                                      Categories =
                                                          browser.List(Find.ById("Categories")).ListItems.SelectMany(
                                                              y => y.Links.Select(x => x.Text.Trim())).ToList(),
                                                      Title =
                                                          browser.ElementWithTag("h2", Find.ById("Title")).Text.Trim()
                                                  },
                               PublishedPost = new PostContent
                                                   {
                                                       Content = browser.Div(Find.ById("Content")).Text.Trim(),
                                                       PublishDate =
                                                           DateTime.Parse(
                                                               browser.Span(Find.ById("PublishDate")).Text.Trim())
                                                   }
                           };
            return post;
        }


        private static Dictionary<string, Action<PostMetaData, string>> configurePostMetaDataDictionary =
            new Dictionary<string, Action<PostMetaData, string>>
                {
                    {"Title", (y, x) => y.Title = x},
                    {"ShortUrl", (y, x) => y.ShortUrl = x},
                    {"Tags", (y, x) => y.Tags = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Categories", (y, x) => y.Categories = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Excerpt", (y, x) => y.Excerpt = x}
                };

        public static IEnumerable<PostMetaData> CreateMetaDataFromTable(Table table)
        {
            return CreateSomethingFromTable(table, configurePostMetaDataDictionary);
        }
    }
}
