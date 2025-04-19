using System;

namespace Lib_System.Models
{
    public class BookLoanViewModel
    {
        public int Id { get; set; }
        public string ReaderName { get; set; }
        public string BookTitle { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
