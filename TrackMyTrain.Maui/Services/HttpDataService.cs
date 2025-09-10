using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;

namespace TrackMyTrain.Maui.Services
{
    public class HttpDataService : IHttpDataService
    {
        private readonly IApiClientService _apiClientService;
        public HttpDataService(IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<TrainJourney> GetTrainJourneyAsync(TrainAutocomplete stationTrain)
        {
            return await _apiClientService.GetAsync<TrainJourney, Void>(string.Format(HttpEndpointConstants.AndamentoTreno, stationTrain.DepartureStationShortCode, stationTrain.TrainNumber, stationTrain.TimeStamp.ToString()), Void.Empty, authRequired: false, isPlainText: false, retry: false);
        }

        public async Task<List<TrainAutocomplete>> GetTrainsByNumberAsync(string trainNumber)
        {
            List<TrainAutocomplete> stationTrains = new List<TrainAutocomplete>();
            var raw = await _apiClientService.GetAsync<string, Void>(string.Format(HttpEndpointConstants.CercaNumeroTreno, trainNumber), Void.Empty, authRequired: false, isPlainText: true);
            foreach (var item in raw.Split("\n"))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var row_1_2 = item.Split("|");
                var row_1 = row_1_2[0];
                var row_2 = row_1_2[1];
                if (trainNumber != row_1.Split("-")[0].Trim())
                    throw new Exception("Train numbers do not match");
                var stationName = row_1.Split("-")[1].Trim();
                var stationShortCode = row_2.Split("-")[1].Trim();
                var timeStamp = row_2.Split("-")[2].Trim();
                stationTrains.Add(new TrainAutocomplete(stationName, trainNumber, stationShortCode, long.Parse(timeStamp)));
            }

            return stationTrains;
        }
    }

    public static class HttpEndpointConstants
    {
        public static string CercaNumeroTreno = "cercaNumeroTrenoTrenoAutocomplete/{0}";
        public static string AndamentoTreno = "andamentoTreno/{0}/{1}/{2}";
    }
}
