using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DeleporterCore.Client;
using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;

namespace NBlog.Specs.Infrastructure
{
    public static class PostRepositoryHelper
    {
        public static IEnumerable<Post> GetPosts()
        {
            var posts = new List<Post>();
            Deleporter.Run(
                () =>
                    {
                        var postRepository = DependencyResolver.Current.GetService<IPostRepository>();
                        posts = postRepository.All().ToList();
                    });
            return posts;
        }
    }
}
