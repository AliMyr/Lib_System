using System;

namespace Lib_System.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ReaderId { get; set; }
        public int RoomId { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
