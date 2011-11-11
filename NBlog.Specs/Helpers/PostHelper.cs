using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Mongo.Repositories;
using NBlog.Specs.Config;

namespace NBlog.Specs.Helpers
{
    public static class PostHelper
    {
        public static void DeletePosts()
        {
            using (var postRepository = new PostRepository(new MongoConfig()))
            {
                postRepository.DeleteAll();
            }
        }
    }
}
