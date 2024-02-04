using Microsoft.Extensions.Configuration;
using System.Data.SQLite;

namespace RitardiTreni.Common.Helpers
{
    public static class SQLiteDbHelper
    {
        public static void CreateDb(IConfiguration configuration)
        {
            try
            {
                SQLiteConnection.CreateFile("db.db");
                var conn = new SQLiteConnection(configuration.GetConnectionString("DefaultConnection"));
                {
                    conn.Open();
                    string strSql = File.ReadAllText(@"Resources\SqliteScript.sql");
                    using var cmd = new SQLiteCommand(conn);
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
