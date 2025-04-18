using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IDbService _db;
        public ReservationService(IDbService db) => _db = db;

        public IEnumerable<Reservation> GetAllReservations()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Reservation>(
                @"SELECT 
                    id, 
                    reader_id AS ReaderId, 
                    room_id AS RoomId, 
                    reservation_date AS ReservationDate, 
                    start_time AS StartTime, 
                    end_time AS EndTime 
                  FROM MA_reading_room_reservation");
        }
    }
}
