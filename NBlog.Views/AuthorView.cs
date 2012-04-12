using System;
using System.Collections.Generic;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public class AuthorView
    {
        private readonly IViewRepository<Author> _authorViewRepository;

        public AuthorView(IViewRepository<Author> authorViewRepository)
        {
            _authorViewRepository = authorViewRepository;
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _authorViewRepository.All();
        }

        public void Handle(UserAddedEvent userAddedEvent)
        {
            var author = new Author()
                             {
                                 AuthorId = userAddedEvent.AdminId,
                                 AuthorName = userAddedEvent.AuthorName
                             };
            _authorViewRepository.Insert(author);
        }
    }

    public class Author
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}