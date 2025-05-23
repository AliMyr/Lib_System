﻿using System.Collections.Generic;
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

        public IEnumerable<ReservationViewModel> GetAllReservationDetails()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<ReservationViewModel>(@"
                SELECT res.id AS Id,
                       CONCAT(r.last_name,' ',r.first_name) AS ReaderName,
                       rr.title AS RoomTitle,
                       res.reservation_date AS ReservationDate,
                       res.start_time AS StartTime,
                       res.end_time AS EndTime
                  FROM MA_reading_room_reservation res
                  JOIN MA_readers r ON res.reader_id = r.id
                  JOIN MA_reading_rooms rr ON res.room_id = rr.id");
        }

        public Reservation GetReservationById(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Reservation>(
                "SELECT id AS Id, reader_id AS ReaderId, room_id AS RoomId, reservation_date AS ReservationDate, start_time AS StartTime, end_time AS EndTime FROM MA_reading_room_reservation WHERE id=@Id",
                new { Id = id });
        }

        public int CreateReservation(Reservation res)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(@"
                INSERT INTO MA_reading_room_reservation (reader_id,room_id,reservation_date,start_time,end_time)
                VALUES (@ReaderId,@RoomId,@ReservationDate,@StartTime,@EndTime);
                SELECT LAST_INSERT_ID();", res);
        }

        public bool UpdateReservation(Reservation res)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(@"
                UPDATE MA_reading_room_reservation
                   SET reader_id=@ReaderId,
                       room_id=@RoomId,
                       reservation_date=@ReservationDate,
                       start_time=@StartTime,
                       end_time=@EndTime
                 WHERE id=@Id", res) > 0;
        }

        public bool DeleteReservation(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_reading_room_reservation WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}