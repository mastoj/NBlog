using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NBlog.Domain.CommandHandlers;
using NBlog.Domain.Commands;
using NBlog.Domain.Entities;
using NBlog.Domain.Event;
using NBlog.Domain.Repositories;
using NUnit.Framework;
using TJ.DDD.Infrastructure;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Respositories;
using TJ.DDD.Infrastructure.Tests;
using TJ.DDD.MongoEvent;

namespace NBlog.Domain.Tests
{
    public class When_Creating_The_Blog : BaseTestSetup
    {
        private StubBlogRepository _blogRepository;

        protected override void Given()
        {
            _blogRepository = new StubBlogRepository();
            var createBlogCommand = new CreateBlogCommand("Title");
            var createBlogCommandHandler = new CreateBlogCommandHandler(_blogRepository);
            createBlogCommandHandler.Execute(createBlogCommand);
        }

        [Test]
        public void It_Should_Create_The_Blog()
        {
            _blogRepository.Blogs.Count.Should().Be(1);
        }
    }

    public class When_Trying_To_Create_The_Blog_When_Already_Created : BaseTestSetup
    {
        protected override void Given()
        {
            IDomainRepository<Blog> blogRepository = new StubBlogRepository();
            var createBlogCommand = new CreateBlogCommand("Title");
            var createBlogCommandHandler = new CreateBlogCommandHandler(blogRepository);
            createBlogCommandHandler.Execute(createBlogCommand);
        }

        [Test]
        public void It_Should_Throw_Blog_Already_Created_Exception()
        {
            CaughtException.Should().BeOfType<BlogAlreadyCreatedException>();
        }
    }

    public class BlogAlreadyCreatedException : Exception
    {
        public BlogAlreadyCreatedException()
        {
            
        }

        public BlogAlreadyCreatedException(string message) : base(message)
        {
            
        }
    }

    public class CreateBlogCommandHandler : IHandle<CreateBlogCommand>
    {
        private readonly IDomainRepository<Blog> _blogRepository;
        private readonly IBlogView _blogView;

        public CreateBlogCommandHandler(IDomainRepository<Blog> blogRepository, IBlogView blogView)
        {
            _blogRepository = blogRepository;
            _blogView = blogView;
        }

        public void Execute(CreateBlogCommand createBlogCommand)
        {
            if (_blogView.GetBlog().IsNotNull())
            {
                throw new BlogAlreadyCreatedException("The blog has already been created");
            }
            var blog = Blog.Create(createBlogCommand.Title, createBlogCommand.AggregateId);
            _blogRepository.Insert(blog);
        }
    }

    public interface IBlogView
    {
    }

    public class CreateBlogCommand : ICommand
    {
        private readonly string _title;
        private Guid _aggregateId;

        public CreateBlogCommand(string title)
        {
            _title = title;
            _aggregateId = Guid.NewGuid();
        }

        public string Title
        {
            get { return _title; }
        }

        public Guid AggregateId
        {
            get { return _aggregateId; }
        }
    }

    [TestFixture]
    public class When_Creating_A_Post : BaseTestSetup
    {
        private string _title;
        private string _shortUrl;
        private CreatePostCommand _createPostCommand;
        private string _excerpt;
        private string _content;
        private List<string> _tags;
        private CreatePostCommandHandler _createPostCommandHandler;
        private StubPostRepository _postRepository;
        private StubBlogRepository _blogRepository;

        protected override void Given()
        {
            _title = "Title";
            _content = "content";
            _shortUrl = "shortUrl";
            _tags = new List<string>() {"tag1", "tag2"};
            _excerpt = "excerpt";
            _createPostCommand = new CreatePostCommand(_title, _content, _shortUrl, _tags, _excerpt);
            _postRepository = new StubPostRepository();
            _blogRepository = new StubBlogRepository();
            _createPostCommandHandler = new CreatePostCommandHandler(_postRepository, _blogRepository);
            _createPostCommandHandler.Execute(_createPostCommand);
        }

        [Test]
        public void The_Post_Should_Be_Created()
        {
            // Assert
            _postRepository.Posts.Count.Should().Be(1);
        }
    }

    public class StubBlogRepository : IDomainRepository<Blog>
    {
        private List<Blog> _blogs = new List<Blog>();
        public List<Blog> Blogs
        {
            get { return _blogs; }
        }

        public void Insert(Blog aggregate)
        {
            _blogs.Add(aggregate);
        }
    }

    public class StubPostRepository : IDomainRepository<Post>
    {
        private List<Post> _posts = new List<Post>();
        public List<Post> Posts
        {
            get { return _posts; }
        }

        public void Insert(Post aggregate)
        {
            _posts.Add(aggregate);
        }
    }
}
