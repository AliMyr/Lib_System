using System;

namespace Lib_System.Models
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string RoleTitle { get; set; }
        public DateTime? HiredDate { get; set; }
        public string Phone { get; set; }
    }
}
