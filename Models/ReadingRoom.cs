﻿namespace Lib_System.Models
{
    public class ReadingRoom
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? Floor { get; set; }
        public int? Capacity { get; set; }
        public bool? HasWiFi { get; set; }
        public int? StaffId { get; set; }
    }
}
