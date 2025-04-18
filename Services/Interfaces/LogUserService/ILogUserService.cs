using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface ILogUserService
    {
        IEnumerable<LogUser> GetAllUsers();
        int CreateUser(LogUser user);
        bool UpdateUser(LogUser user);
        bool DeleteUser(int id);
    }
}
