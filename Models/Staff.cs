using System;

namespace Lib_System.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? HiredDate { get; set; }
        public string Phone { get; set; }
    }
}
