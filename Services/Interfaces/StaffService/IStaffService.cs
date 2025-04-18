using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IStaffService
    {
        IEnumerable<Staff> GetAllStaff();
        int CreateStaff(Staff staff);
        bool UpdateStaff(Staff staff);
        bool DeleteStaff(int id);
    }
}