using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TrackMyTrain.Maui.Services
{
    public class HttpDataService : IHttpDataService
    {
        string pattern = @"modalità:|Data fine:|Settore:|Rilevanza:|Regione:|Provincia:|Sindacati:|Categoria interessata:|Data proclamazione:|Data ricezione:";
        private readonly IApiClientService _apiClientService;
        public HttpDataService(IApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<List<ExtendedFeedItem>> GetStrikesAsync()
        {
            _apiClientService.ClientName = "rssfeed";
            var xmlString = await _apiClientService.GetAsync<string, Void>(HttpEndpointConstants.RssFeedScioperi, Void.Empty, authRequired: false, isPlainText: true, retry: false);
            var doc = XDocument.Parse(xmlString);

            var items = doc.Descendants("item")
                           .Select(x =>
                           {
                               var match = Regex.Match((string)x.Element("title"), @"\b\d{2}/\d{2}/\d{4}\b");
                               if (match.Success)
                               {
                                   string date = match.Value;

                                   string[] tokensDescription = Regex.Split((string)x.Element("description"), pattern, RegexOptions.IgnoreCase);
                                   return new ExtendedFeedItem
                                   {
                                       DataInizio = date.Trim(),
                                       Modalita = tokensDescription[1].Trim().Replace("<br/>", ""),
                                       DataFine = tokensDescription[2].Trim().Replace("<br/>", ""),
                                       Settore = tokensDescription[3].Trim().Replace("<br/>", ""),
                                       Rilevanza = tokensDescription[4].Trim().Replace("<br/>", ""),
                                       Regione = tokensDescription[5].Trim().Replace("<br/>", ""),
                                       Provincia = tokensDescription[6].Trim().Replace("<br/>", ""),
                                       Sindacati = tokensDescription[7].Trim().Replace("<br/>", ""),
                                       CategoriaInteressata = tokensDescription[8].Trim().Replace("<br/>", ""),
                                       DataProclamazione = tokensDescription[9].Trim().Replace("<br/>", ""),
                                       DataRicezione = tokensDescription[10].Trim().Replace("<br/>", ""),
                                   };
                               }
                               else
                               {
                                   throw new Exception("Could not parse Rss feed");
                               }
                           })
                           .ToList();

            return items;
        }

        public async Task<TrainJourney> GetTrainJourneyAsync(TrainAutocomplete stationTrain)
        {
            _apiClientService.ClientName = "api";
            return await _apiClientService.GetAsync<TrainJourney, Void>(string.Format(HttpEndpointConstants.AndamentoTreno, stationTrain.DepartureStationShortCode, stationTrain.TrainNumber, stationTrain.TimeStamp.ToString()), Void.Empty, authRequired: false, isPlainText: false, retry: false);
        }

        public async Task<List<TrainAutocomplete>> GetTrainsByNumberAsync(string trainNumber)
        {
            _apiClientService.ClientName = "api";
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
        public static string RssFeedScioperi = "mit2/public/scioperi/rss";
    }
}
