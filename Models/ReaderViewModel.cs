using System;

namespace Lib_System.Models
{
    public class ReaderViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
