using RitardiTreni.Common.Model;

namespace RitardiTreni.Common.Services
{
    public interface IDbContextService
    {
        Task<bool> CheckDateCacheValidityAsync();
        Task AddNewLine(LineDto value);
        Task<IEnumerable<LineDto>> GetAllLines();
        Task AddNewTrainLine(TrainLineDto value);
        Task<IEnumerable<LineDto>> GetAllTrainLines();
    }
}