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
            var posts = new List<Post>();
            foreach (var row in table.Rows)
            {
                var post = CreatePostFromRow(row);
                posts.Add(post);
            }
            return posts;
        }

        private static Dictionary<string, Action<Post, string>> configurePostDictionary =
            new Dictionary<string, Action<Post, string>>
                {
                    {"Title", (y, x) => y.Title = x},
                    {"ShortUrl", (y, x) => y.ShortUrl = x},
                    {"Content", (y, x) => y.Content = x},
                    {"PublishDate", (y, x) => y.PublishDate = ParseDate(x)},
                    {"Publish", (y, x) => y.Publish = bool.Parse(x)},
                    {"Tags", (y, x) => y.Tags = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Categories", (y, x) => y.Categories = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Excerpt", (y, x) => y.Excerpt = x}
                };

        private static DateTime? ParseDate(string wantedDate)
        {
            if (wantedDate.IsNullOrEmpty())
            {
                return null;
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

        private static Post CreatePostFromRow(TableRow row)
        {
            var post = new Post();
            foreach (var key in row.Keys)
            {
                var value = row[key];
                if (configurePostDictionary.ContainsKey(key).IsTrue())
                {
                    configurePostDictionary[key](post, value);
                }
                else
                {
                    Assert.Fail("Missing key: {0}", key);
                }
            }
            return post;
        }

        public static IEnumerable<Post> GetPostsFromRegularListing(ObservableBrowser browser)
        {
            var postEntryContainer = browser.Div(Find.ByClass("posts"));
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
            var post = new Post();
            var postLink = div.Link(Find.ByClass("post-link")).Url;
            post.ShortUrl = new Uri(postLink).Segments.LastOrDefault();
            var excerptSpan = div.Span(Find.ByClass("excerpt"));
            post.Excerpt = excerptSpan.InnerHtml;
            return post;
        }
    }
}
