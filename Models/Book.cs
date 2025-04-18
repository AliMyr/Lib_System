using System;

namespace Lib_System.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublisherId { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int Pages { get; set; }
        public int GenreId { get; set; }
        public int LanguageId { get; set; }
    }
}
