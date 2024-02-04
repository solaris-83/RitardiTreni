using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary.Helpers
{
    public static class SQLiteDbHelper
    {
        public static void CreateDb(IConfiguration configuration)
        {
            SQLiteConnection.CreateFile("myDb.db");
            var conn = new SQLiteConnection(configuration.GetConnectionString("DefaultConnection"));
            {
                conn.Open();
                string strSql = File.ReadAllText(@"Resources\SqliteScript.sql");
                using var cmd = new SQLiteCommand(conn);
                cmd.CommandText = strSql;
                cmd.ExecuteNonQuery();
            };
        }
    }
}
