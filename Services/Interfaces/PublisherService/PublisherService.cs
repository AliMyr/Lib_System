using System.Collections.Generic;
using System.Data;
using Dapper;
using Lib_System.Models;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IDbService _db;
        public PublisherService(IDbService db) => _db = db;

        public Publisher GetPublisherById(int id)
        {
            using var c = _db.GetConnection();
            c.Open();
            return c.QuerySingle<Publisher>(
                "SELECT id AS Id, title AS Title, country AS Country FROM MA_publishers WHERE id=@Id",
                new { Id = id });
        }

        public IEnumerable<PublisherViewModel> GetAllPublisherDetails()
        {
            using var c = _db.GetConnection();
            c.Open();
            return c.Query<PublisherViewModel>(
                @"SELECT id AS Id, title AS Title, country AS Country
                    FROM MA_publishers");
        }

        public int CreatePublisher(Publisher p)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.ExecuteScalar<int>(
                @"INSERT INTO MA_publishers (title,country)
                  VALUES (@Title,@Country);
                  SELECT LAST_INSERT_ID();", p);
        }

        public bool UpdatePublisher(Publisher p)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute(
                @"UPDATE MA_publishers
                  SET title=@Title, country=@Country
                  WHERE id=@Id", p) > 0;
        }

        public bool DeletePublisher(int id)
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Execute("DELETE FROM MA_publishers WHERE id=@Id", new { Id = id }) > 0;
        }
    }
}
