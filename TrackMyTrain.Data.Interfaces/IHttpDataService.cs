using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Data.Implementations;

namespace TrackMyTrain.Data.Interfaces
{
    public interface IHttpDataService
    {
        Task<List<ExtendedFeedItem>> GetStrikesAsync();
        Task<List<TrainAutocomplete>> GetTrainsByNumberAsync(string trainNumber);

        Task<TrainJourney> GetTrainJourneyAsync(TrainAutocomplete stationTrain);
    }
}
