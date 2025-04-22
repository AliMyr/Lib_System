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

        public IEnumerable<ReadingRoomViewModel> GetAllRoomDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<ReadingRoomViewModel>(@"
                SELECT rr.id AS Id,
                       rr.title AS Title,
                       rr.floor AS Floor,
                       rr.capacity AS Capacity,
                       rr.has_wi_fi AS HasWiFi,
                       CONCAT(s.last_name,' ',s.first_name) AS StaffName
                  FROM MA_reading_rooms rr
                  LEFT JOIN MA_staff s ON rr.staff_id = s.id");
        }

        public ReadingRoom GetRoomById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<ReadingRoom>(
                "SELECT id AS Id, title AS Title, floor AS Floor, capacity AS Capacity, has_wi_fi AS HasWiFi, staff_id AS StaffId FROM MA_reading_rooms WHERE id=@Id",
                new { Id = id });
        }

        public int CreateRoom(ReadingRoom room)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(@"
                INSERT INTO MA_reading_rooms (title,floor,capacity,has_wi_fi,staff_id)
                VALUES (@Title,@Floor,@Capacity,@HasWiFi,@StaffId);
                SELECT LAST_INSERT_ID();", room);
        }

        public bool UpdateRoom(ReadingRoom room)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_reading_rooms
                   SET title=@Title,
                       floor=@Floor,
                       capacity=@Capacity,
                       has_wi_fi=@HasWiFi,
                       staff_id=@StaffId
                 WHERE id=@Id", room) > 0;
        }

        public bool DeleteRoom(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_reading_rooms WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}