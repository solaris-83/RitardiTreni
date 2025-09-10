
using TrackMyTrain.Data.Implementations;

namespace TrackMyTrain.Data.Interfaces
{
    public interface IDataService
    {
        List<StationLocation> CachedStations { get; }
        Task<List<StationLocation>> LoadCachedStationsAsync();
    }
}
