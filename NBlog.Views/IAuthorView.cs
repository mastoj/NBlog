using System.Collections.Generic;
using NBlog.Domain.Event;

namespace NBlog.Views
{
    public interface IAuthorView
    {
        IEnumerable<Author> GetAuthors();
        void Handle(UserAddedEvent userAddedEvent);
        Author GetAuthor(string identity);
    }
}