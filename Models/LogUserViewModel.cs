﻿using System;

namespace Lib_System.Models
{
    public class LogUserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
