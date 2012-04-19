using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public class AuthorView : IAuthorView
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
                                 AuthorName = userAddedEvent.AuthorName,
                                 AuthorEmail = userAddedEvent.Email
                             };
            _authorViewRepository.Insert(author);
        }

        public Author GetAuthor(string identity)
        {
            return _authorViewRepository.All().SingleOrDefault(y => y.AuthorId == identity);
        }
    }

    public class Author
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
    }
}