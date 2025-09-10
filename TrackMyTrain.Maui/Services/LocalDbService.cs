
using SQLite;
using System.Linq.Expressions;
using TrackMyTrain.Maui.LocalDb.Models;

namespace TrackMyTrain.Maui.Services
{
    public class LocalDbService : IAsyncDisposable
    {
        
            private const string DbName = "TrackMyTrain.db3";
            private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, DbName);

            private SQLiteAsyncConnection _connection;
            private SQLiteAsyncConnection Database =>
                (_connection ??= new SQLiteAsyncConnection(DbPath,
                    SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));

            private async Task CreateTableIfNotExists<TTable>() where TTable : TableBase, new()
            {
                await Database.CreateTableAsync<TTable>();
            }

            private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                return Database.Table<TTable>();
            }

            public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : TableBase, new()
            {
                var table = await GetTableAsync<TTable>();
                return await table.ToListAsync();
            }

            public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : TableBase, new()
            {
                var table = await GetTableAsync<TTable>();
                return await table.Where(predicate).ToListAsync();
            }

            private async Task<TResult> Execute<TTable, TResult>(Func<Task<TResult>> action) where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                return await action();
            }

            public async Task<TTable> GetItemByKeyAsync<TTable>(object primaryKey) where TTable : TableBase, new()
            {
                //await CreateTableIfNotExists<TTable>();
                //return await Database.GetAsync<TTable>(primaryKey);
                return await Execute<TTable, TTable>(async () => await Database.GetAsync<TTable>(primaryKey));
            }

            public async Task<bool> AddItemAsync<TTable>(TTable item) where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                //return await Database.InsertAsync(item) > 0;
                item.CreatedAt = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                item.LastUpdatedAt = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                return await Execute<TTable, bool>(async () => await Database.InsertAsync(item) > 0);
            }

            public async Task<bool> UpdateItemAsync<TTable>(TTable item) where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                item.LastUpdatedAt = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
                return await Database.UpdateAsync(item) > 0;
            }

            public async Task<bool> DeleteItemAsync<TTable>(TTable item) where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                return await Database.DeleteAsync(item) > 0;
            }

            public async Task<bool> DeleteItemByKeyAsync<TTable>(object primaryKey) where TTable : TableBase, new()
            {
                await CreateTableIfNotExists<TTable>();
                return await Database.DeleteAsync<TTable>(primaryKey) > 0;
            }

            public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
        
    }
}
