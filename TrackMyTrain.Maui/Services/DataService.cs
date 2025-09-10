using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;

namespace TrackMyTrain.Maui.Services
{
    public class DataService : IDataService
    {
        public List<StationLocation> CachedStations { get; private set; } = new List<StationLocation>();
        public async Task<List<StationLocation>> LoadCachedStationsAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("stations.raw");
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();
            foreach (var row in contents.Split("\n"))
            {
                var station = row.Split("|");
                //  _logger.LogInformation($"Loaded station: {row}");
                CachedStations.Add(new StationLocation(station[0], station[1]));
            }

            return CachedStations;
        }
    }
}
