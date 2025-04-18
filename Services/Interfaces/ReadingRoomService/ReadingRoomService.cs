using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class ReadingRoomService : IReadingRoomService
    {
        private readonly IDbService _db;
        public ReadingRoomService(IDbService db) => _db = db;

        public IEnumerable<ReadingRoom> GetAllRooms()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<ReadingRoom>(
                @"SELECT 
                    id, 
                    title AS Title, 
                    floor AS Floor, 
                    capacity AS Capacity, 
                    has_wi_fi AS HasWiFi, 
                    staff_id AS StaffId 
                  FROM MA_reading_rooms");
        }
    }
}
