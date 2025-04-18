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
        public IEnumerable<Publisher> GetAllPublishers()
        {
            using IDbConnection c = _db.GetConnection();
            c.Open();
            return c.Query<Publisher>(
                "SELECT id, title AS Title, country AS Country FROM MA_publishers");
        }
    }
}