using System;

namespace Lib_System.Models
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public string ReaderName { get; set; }
        public string RoomTitle { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
