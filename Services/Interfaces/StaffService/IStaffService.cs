using System.Collections.Generic;
using Lib_System.Models;

namespace Lib_System.Services.Interfaces
{
    public interface IStaffService
    {
        IEnumerable<StaffViewModel> GetAllStaffDetails();
        Staff GetStaffById(int id);
        int CreateStaff(Staff staff);
        bool UpdateStaff(Staff staff);
        bool DeleteStaff(int id);
    }
}