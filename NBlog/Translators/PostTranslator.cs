using System;
using System.Collections.Generic;
using NBlog.Domain;
using NBlog.Domain.Entities;

namespace NBlog.Translators
{
    public static class PostTranslator
    {
        public static Post ToDTO(this IPost post)
        {
            var postDto = new Post()
                              {
                                  Categories = post.Categories,
                                  Content = post.Content,
                                  Id = post.Id,
                                  Publish = post.Publish,
                                  PublishDate = post.PublishDate,
                                  ShortUrl = post.ShortUrl,
                                  Tags = post.Tags,
                                  Title = post.Title,
                                  Excerpt = post.Excerpt
                              };
            return postDto;
        }

        public static T ToIPost<T>(this Post postDto) where T : IPost, new()
        {
            var currentTimeZone = TimeZone.CurrentTimeZone;
            var post = new T()
                           {
                               Categories = postDto.Categories,
                               Content = postDto.Content,
                               Id = postDto.Id,
                               Publish = postDto.Publish,
                               PublishDate = currentTimeZone.ToLocalTime(postDto.PublishDate),
                               ShortUrl = postDto.ShortUrl,
                               Tags = postDto.Tags,
                               Title = postDto.Title,
                               Excerpt = postDto.Excerpt
                           };
            return post;
        }

        public static IEnumerable<T> ToIPosts<T>(this IEnumerable<Post> postDtos) where T : IPost, new()
        {
            foreach (var postDto in postDtos)
            {
                var iPost = postDto.ToIPost<T>();
                yield return iPost;
            }
        }
    }
}
