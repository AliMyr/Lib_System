using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IAuthorBookService
    {
        IEnumerable<AuthorBookViewModel> GetAllRelationsDetailed();
        AuthorBook GetRelationById(int id);
        int CreateRelation(AuthorBook rel);
        bool UpdateRelation(AuthorBook rel);
        bool DeleteRelation(int id);
    }
}
