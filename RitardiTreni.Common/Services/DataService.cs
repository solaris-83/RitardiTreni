using System.Text.RegularExpressions;
using RitardiTreni.Common.Model;
using RitardiTreni.Common.Responses;
using RestSharp;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Net;

namespace RitardiTreni.Common.Services
{
    public class DataService : IDataService
    {
        DataItemExtended dataList;
        private const string BASE_PATH_1 = BASE_PATH + "/resteasy/viaggiatreno";
        private const string BASE_PATH = "http://www.viaggiatreno.it/infomobilita";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public DataService(IHttpClientFactory httpClientFactory, ILogger<DataService> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            dataList = new DataItemExtended();
        }

        public async Task<string> ShowArrivalsAsync(string numeroTreno, string nomeStazione, DateTime dataSelezionata)
        {
            string comunicazione = "";
            using (var httpClient = _httpClientFactory.CreateClient("Resteasy"))
            {
                string uri = $@"autocompletaStazione/{(nomeStazione.Length > 20 ? nomeStazione.Substring(0, 20) : nomeStazione)}";
                var httpResponseMessage = await httpClient.GetAsync(uri);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseToParse = await httpResponseMessage.Content.ReadAsStringAsync();
                    responseToParse?.TrimEnd('\r', '\n');
                    if (string.IsNullOrEmpty(responseToParse))
                        return comunicazione;
                    var info_1 = responseToParse.Split('\n')[0].Split('|');
                    if (info_1[0] != nomeStazione)
                        return comunicazione;
                    var info_2 = info_1[1].Split('-');

                    var baseAddress = new Uri("http://www.viaggiatreno.it/infomobilita/");
                    var cookieContainer = new CookieContainer();
                    using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
                    using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
                    { 
                        var queryParameters = new Dictionary<string, string>
                        {
                            { "numTreno", numeroTreno },
                            { "locArrivo", info_2[0]},
                            { "locArrivoDesc", nomeStazione },
                            { "date", dataSelezionata.ToString("dd-MM-yyyy")}
                        };
                        client.DefaultRequestHeaders.Add("Accept", "text/plain");
                        var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
                        var httpResponseMessage1 = await client.PostAsync("StampaTreno", dictFormUrlEncoded);
                        if (httpResponseMessage1.IsSuccessStatusCode)
                        {
                            using var resp = await httpResponseMessage1.Content.ReadAsStreamAsync();
                            var c = cookieContainer.GetAllCookies();
                            var jsessionID = c.SingleOrDefault(c => c.Name == "JSESSIONID")?.Value;
                            if (!string.IsNullOrWhiteSpace(jsessionID))
                            {
                                httpResponseMessage1 = await client.PostAsync("StampaTreno", dictFormUrlEncoded);
                                if (httpResponseMessage1.IsSuccessStatusCode)
                                {
                                    var response1 = await httpResponseMessage1.Content.ReadAsStringAsync();
                                    var com = JsonSerializer.Deserialize<ComunicazioneArrivo>(response1);
                                    comunicazione = com?.comunicazione ?? "Errore";
                                }
                            }
                            else
                            {
                                comunicazione = "Errore JSESSIONID";
                            }
                        }
                        else
                            comunicazione = "Errore";
                    }
                }
            }
            return comunicazione?.Replace("&egrave;", "è") ?? "";
        }

        public async Task<DataItemExtended> GetInfoByTrainAsync(List<string> codiceStazionePartenza, List<string> codiceStazioneArrivo, string pattern)
        {
            using (var httpClient = _httpClientFactory.CreateClient("Resteasy"))
            {
                var elencoTreni = new List<(string, string)>();
                for (int i = 0; i < codiceStazionePartenza.Count; i++)
                {
                    var dataOra = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    string uri = $@"soluzioniViaggioNew/{codiceStazionePartenza[i].TrimStart(new char[] { 'S', '0' })}/{codiceStazioneArrivo[i].TrimStart(new char[] { 'S', '0' })}/{DateTime.Now.ToString("yyyy-MM-ddT00:00:00")}";
                    var httpResponseMessage = await httpClient.GetAsync(uri);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                        var response = await JsonSerializer.DeserializeAsync<ElencoNumeriTreni>(contentStream);
                        response?.soluzioni?.ForEach(sol => sol.vehicles.ForEach(veh =>
                        {
                            if ((veh.categoria == "235" && veh.categoriaDescrizione == "RV") || (veh.categoria == "197" && veh.categoriaDescrizione == "Regionale"))
                            {
                                if (!elencoTreni.Contains((veh.numeroTreno, codiceStazionePartenza[i])) && Regex.IsMatch(veh.numeroTreno, pattern))
                                {
                                    elencoTreni.Add((veh.numeroTreno, codiceStazionePartenza[i]));
                                }
                            }
                        }));

                        elencoTreni.OrderBy(s => s.Item1);
                    }
                }

                dataList = new DataItemExtended();
                foreach (var treno in elencoTreni)
                {
                    var uri = $@"andamentoTreno/{treno.Item2}/{treno.Item1}/{DateTimeToUnixTimeStamp(DateTime.Today)}";
                    var httpResponseMessage = await httpClient.GetAsync(uri);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                        try
                        {
                            var response = await JsonSerializer.DeserializeAsync<OrarioTreni>(contentStream);
                            if (response != null)
                            {
                                if (response.fermate != null)
                                {
                                    string[] input = new string[3] { response.tipoTreno, response.provvedimento.ToString(), response.subTitle };
                                    var statoTreno = GetStatoTreno(input);
                                    if (response.fermate.Count > 0)
                                    {
                                        response.fermate.ForEach(f =>
                                            {
                                                dataList.DataList.Add(new DataItem(treno.Item1, f.stazione, f.ritardo, string.IsNullOrEmpty(statoTreno) ? UnixTimeStampToDateTime(double.Parse(f.programmata.ToString())).ToString("HH:mm") : statoTreno, f.tipoFermata == "A" || f.tipoFermata == "P"));
                                            });
                                    }
                                    else
                                    {
                                        dataList.DataList.Add(new DataItem(treno.Item1, "", 0, string.IsNullOrEmpty(statoTreno) ? "" : statoTreno, true));
                                    }
                                }

                                dataList.OraUltimoRilevamento = response.oraUltimoRilevamento != null ? UnixTimeStampToDateTime(double.Parse(response.oraUltimoRilevamento.ToString())).ToString("HH:mm") : "";
                                dataList.StazioneUltimoRilevamente = response.stazioneUltimoRilevamento;
                                dataList.Categoria = response.categoria;
                                dataList.NumeroTreno = response.numeroTreno.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Andamento in errore per treno {treno.Item1} e codice stazione {treno.Item2}");
                        }
                    }
                }
            }
            return dataList;
        }

        private string GetStatoTreno(string[] input)
        {
            if (input[0] == "PG" && input[1] == "0")
            {
                return "";
            }
            if (input[0] == "ST" && input[1] == "1")
            {
                return "SOPPRESSO";
            }
            if ((input[0] == "PP" || input[0] == "SI" || input[0] == "SF" || input[0] == "RF") && (input[1] == "0" || input[1] == "2"))
            {
                return string.Format("PARZ. SOPPRESSO - {0}", input[2]);
            }
            if (input[0] == "DV" && input[1] == "3")
            {
                return "DEVIATO";
            }
            return "";
        }

        private long DateTimeToUnixTimeStamp(DateTime dt)
        {
            return ((DateTimeOffset)dt).ToUnixTimeMilliseconds();
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
