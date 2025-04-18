using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles();
        int CreateRole(Role role);
        bool UpdateRole(Role role);
        bool DeleteRole(int id);
    }
}
