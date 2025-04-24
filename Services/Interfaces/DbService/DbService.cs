using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using Lib_System.Services.Interfaces;

namespace Lib_System.Services
{
    public class DbService : IDbService
    {
        private readonly string _connectionString;
        public DbService()
        {
            //_connectionString = ConfigurationManager.ConnectionStrings["LocalMySqlConnection"].ConnectionString;
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultMySql"].ConnectionString;
        }

        public IDbConnection GetConnection() => new MySqlConnection(_connectionString);
    }
}
