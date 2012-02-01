using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NBlog.Areas.Admin.Models;
using NBlog.Domain.Builders;
using NBlog.Domain.Entities;
using NBlog.Domain.Repositories;
using NBlog.Domain;
using NBlog.Helpers;
using NBlog.Translators;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public partial class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IBuild<Post> _postBuilder;

        public PostController(IPostRepository postRepository, IBuild<Post> postBuilder)
        {
            _postRepository = postRepository;
            _postBuilder = postBuilder;
        }

        //
        // GET: /Post/

        public virtual ViewResult Index()
        {
            var model = _postRepository.All().Select(y => new ListPostViewModel() { Post = y});
            return View(Views.Index, model);
        }

        public virtual ActionResult Create()
        {
            return View(Views.Create, new PostViewModel());
        }

        [HttpPost]
        public virtual ActionResult Create(PostViewModel model)
        {
            if (ModelState.IsValid.IsFalse())
            {
                ViewData.AddErrorMessage("Failed to create post");
                return View(Views.Create, model);
            }
            else
            {
                SavePost(model);
                ViewData.AddSuccessMessage("Post created!");
                return View(Views.Create);
            }
        }

        public void SavePost(PostViewModel postViewModel, DateTime? publishDate = null, bool? isPublished = null)
        {
            isPublished = isPublished ?? false;
            publishDate = publishDate ?? DateTime.Now;
            var postContent = new PostContent
            {
                Content = postViewModel.Content,
                IsPublished = isPublished.Value,
                PublishDate = publishDate.Value,

            };

            var post = _postBuilder.Build();
            post.PostMetaData = new PostMetaData
                                    {
                                        Categories = postViewModel.Categories,
                                        Excerpt = postViewModel.Excerpt,
                                        ShortUrl = postViewModel.ShortUrl,
                                        Title = postViewModel.Title,
                                        Tags = postViewModel.Tags
                                    };
            post.PostVersions = new List<PostContent>
                                    {
                                        postContent
                                    };
            post.PublishedPost = postContent;
            post.CreateOrUpdate();
        }

        public virtual ActionResult Edit(string id)
        {
            var post = _postRepository.Single(y => y.PostMetaData.ShortUrl == id);
            if (post.IsNull())
            {
                return new HttpNotFoundResult("Can't find post");
            }
            var model = ToPostViewModel(post);
            return View(Views.Edit, model);
        }

        PostViewModel ToPostViewModel(Post post)
        {
            var postViewModel = new PostViewModel
                                    {
                                        Categories = post.PostMetaData.Categories,
                                        Content = post.PostVersions.First().Content,
                                        Excerpt = post.PostMetaData.Excerpt,
                                        Publish = post.PublishedPost.IsNotNull(),
                                        PublishDate =
                                            post.PublishedPost.IsNotNull()
                                                ? post.PublishedPost.PublishDate
                                                : DateTime.MinValue,
                                        ShortUrl = post.PostMetaData.ShortUrl,
                                        Tags = post.PostMetaData.Tags,
                                        Title = post.PostMetaData.Title
                                    };
            return postViewModel;
        }
    }
}

