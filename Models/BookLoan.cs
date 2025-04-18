using System;

namespace Lib_System.Models
{
    public class BookLoan
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int BookCopiesId { get; set; }
        public DateTime? LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
