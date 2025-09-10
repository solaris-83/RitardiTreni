using TrackMyTrain.Maui.LocalDb.Models;

namespace TrackMyTrain.Maui.Services
{
    public interface ILocalDbService<T> where T : TableBase, new()
    {

        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<int> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
        Task<int> DeleteByIdAsync(object id);

    }
}