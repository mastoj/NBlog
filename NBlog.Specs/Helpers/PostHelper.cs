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
using TechTalk.SpecFlow;

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
                    {"PublishDate", (y, x) => y.PublishDate = DateTime.Parse(x)},
                    {"Publish", (y, x) => y.Publish = bool.Parse(x)},
                    {"Tags", (y, x) => y.Tags = x.Split(',').Select(z => z.Trim()).ToList()},
                    {"Categories", (y, x) => y.Categories = x.Split(',').Select(z => z.Trim()).ToList()}
                };
        private static Post CreatePostFromRow(TableRow row)
        {
            var post = new Post();
            foreach (var key in row.Keys)
            {
                var value = row[key];
                if(configurePostDictionary.ContainsKey(key).IsTrue())
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
    }
}
