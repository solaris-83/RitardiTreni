using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using RitardiTreni.Common.Model;

namespace RitardiTreni.Common.Services
{
    public class DbContextService : IDbContextService
    {
        private IConfiguration _configuration;
        public DbContextService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task<bool> CheckDateCacheValidityAsync()
        {
            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<bool>("SELECT IFNULL(Value, 0) FROM DateCache WHERE date > datetime('now')");
        }

        public async Task AddNewLine(LineDto value)
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync("INSERT INTO Lines(Name) VALUES @Name", new { value.Name });
        }

        public async Task<IEnumerable<LineDto>> GetAllLines()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<LineDto>("SELECT Id, Name FROM Lines");
        }

        public async Task AddNewTrainLine(TrainLineDto value)
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync("INSERT INTO TrainLines(Number, LineId) VALUES @Number, @LineId", new { value.Number, value.LineId });
        }

        public async Task<IEnumerable<LineDto>> GetAllTrainLines()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<LineDto>("SELECT Id, Number, LineId FROM TrainLines");
        }

        private IDbConnection GetConnection() => new SQLiteConnection(_configuration.GetConnectionString("DefaultConnection"));
    }
}
