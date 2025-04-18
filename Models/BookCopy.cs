namespace Lib_System.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int? ReadingRoomsId { get; set; }
    }
}
