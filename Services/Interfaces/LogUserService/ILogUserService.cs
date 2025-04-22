using Lib_System.Models;
using System.Collections.Generic;

public interface ILogUserService
{
    IEnumerable<LogUserViewModel> GetAllUserDetails();
    LogUser GetUserById(int id);
    int CreateUser(LogUser user);
    bool UpdateUser(LogUser user);
    bool DeleteUser(int id);
}
