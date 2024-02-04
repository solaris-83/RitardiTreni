using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace DataServiceLibrary.Services
{
    public class DbContextService : IDbContextService
    {
        private IConfiguration _configuration;
        public DbContextService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection() => new SQLiteConnection(_configuration.GetConnectionString("DefaultConnection"));
        public void CheckOrUpdateDateCacheValidity(DateTime dateTime)
        {
            using var conn = GetConnection();
            conn.ExecuteAsync("");
        }
    }
}
