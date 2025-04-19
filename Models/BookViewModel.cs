using System;

namespace Lib_System.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PublisherName { get; set; }
        public string GenreTitle { get; set; }
        public string LanguageTitle { get; set; }
        public DateTime? PublicationDate { get; set; }
        public int Pages { get; set; }
    }
}
