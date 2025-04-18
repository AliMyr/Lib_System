using System;

namespace Lib_System.Models
{
    public class Reader
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
