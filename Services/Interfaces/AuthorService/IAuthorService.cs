using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IAuthorService
    {
        IEnumerable<AuthorViewModel> GetAllAuthorDetails();
        IEnumerable<Author> GetAllAuthors();
        Author GetAuthorById(int id);
        int CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(int id);
    }
}
