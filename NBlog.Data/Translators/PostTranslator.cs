﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Data.DTO;

namespace NBlog.Data.Translators
{
    public static class PostTranslator
    {
        public static Post ToDTO(IPost post)
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
                                  Title = post.Title
                              };
            return postDto;
        }

        public static T ToIPost<T>(this Post postDto) where T : IPost, new()
        {
            var post = new T()
                           {
                               Categories = postDto.Categories,
                               Content = postDto.Content,
                               Id = postDto.Id,
                               Publish = postDto.Publish,
                               PublishDate = postDto.PublishDate,
                               ShortUrl = postDto.ShortUrl,
                               Tags = postDto.Tags,
                               Title = postDto.Title
                           };
            return post;
        }
    }
}
