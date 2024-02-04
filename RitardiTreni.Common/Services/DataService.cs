using System.Text.RegularExpressions;
using RitardiTreni.Common.Model;
using RitardiTreni.Common.Responses;
using RestSharp;
using System.Text.Json;
using System.Net.Http;

namespace RitardiTreni.Common.Services
{
    public class DataService : IDataService
    {
        List<(string, string)> _elencoTreni;
        DataItemExtended dataList;
        private const string BASE_PATH_1 = BASE_PATH + "/resteasy/viaggiatreno";
        private const string BASE_PATH = "http://www.viaggiatreno.it/infomobilita";
        private readonly IHttpClientFactory _httpClientFactory;
        public DataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _elencoTreni = new List<(string, string)>();
            dataList = new DataItemExtended();
        }

        private async Task GetStationsAsync(HttpClient httpClient, string codiceStazionePartenza, string codiceStazioneArrivo, bool isRecursive, string pattern)
        {
            var dataOra = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            string uri = $@"soluzioniViaggioNew/{codiceStazionePartenza.TrimStart(new char[] { 'S', '0' })}/{codiceStazioneArrivo.TrimStart(new char[] { 'S', '0' })}/{DateTime.Now.ToString("yyyy-MM-ddT00:00:00")}";
            var httpResponseMessage = await httpClient.GetAsync(uri);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                var response = await JsonSerializer.DeserializeAsync<ElencoNumeriTreni>(contentStream);
                response?.soluzioni?.ForEach(sol => sol.vehicles.ForEach(veh =>
                {
                    if ((veh.categoria == "235" && veh.categoriaDescrizione == "RV") || (veh.categoria == "197" && veh.categoriaDescrizione == "Regionale"))
                    {
                        if (!_elencoTreni.Contains((veh.numeroTreno, codiceStazionePartenza)) && Regex.IsMatch(veh.numeroTreno, pattern))
                            _elencoTreni.Add((veh.numeroTreno, codiceStazionePartenza));
                    }
                }));

                if (!isRecursive)
                {
                    _elencoTreni.OrderBy(s => s.Item1);
                    return;
                }
                await GetStationsAsync(httpClient, codiceStazioneArrivo, codiceStazionePartenza, false, pattern);
                _elencoTreni.OrderBy(s => s.Item1);
            }
        }


        public async Task<string> ShowArrivalsAsync(string numeroTreno, string nomeStazione, DateTime dataSelezionata)
        {
            string comunicazione = "";
            using (var clientTreno = new RestClient(BASE_PATH_1))
            {
                var requestTreno = new RestRequest(@"autocompletaStazione/{stazione}")
                {
                    RequestFormat = DataFormat.None
                };
                requestTreno.AddHeader("Accept", "text/plain");
                requestTreno.AddUrlSegment("stazione", nomeStazione.Length > 20 ? nomeStazione.Substring(0, 20) : nomeStazione);

                var responseTreno = await clientTreno.GetAsync(requestTreno);
                var responseToParse = responseTreno.Content?.ToString().TrimEnd('\r', '\n');
                if (string.IsNullOrEmpty(responseToParse))
                    return comunicazione;
                var info_1 = responseToParse.Split('\n')[0].Split('|');
                if (info_1[0] != nomeStazione)
                    return comunicazione;
                var info_2 = info_1[1].Split('-');
                using (var client = new RestClient(BASE_PATH))
                {
                    var request = new RestRequest("StampaTreno");

                    request.AddQueryParameter("numTreno", numeroTreno);
                    request.AddQueryParameter("locArrivo", info_2[0]);
                    request.AddQueryParameter("locArrivoDesc", nomeStazione);
                    request.AddQueryParameter("date", dataSelezionata.ToString("dd-MM-yyyy"));

                    var response = await client.PostAsync(request);
                    if (response != null)
                    {

                        var cookie = response.Cookies["JSESSIONID"];
                        if (cookie != null)
                            request.AddCookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain);
                        if (string.IsNullOrEmpty(comunicazione))
                        {
                            request.AddQueryParameter("numTreno", numeroTreno);
                            request.AddQueryParameter("locArrivo", info_2[0]);
                            request.AddQueryParameter("locArrivoDesc", nomeStazione);
                            request.AddQueryParameter("date", dataSelezionata.ToString("dd-MM-yyyy"));
                            var response1 = await client.PostAsync<ComunicazioneArrivo>(request);
                            comunicazione = response1?.comunicazione ?? "";
                        }
                    }
                    else
                        comunicazione = "Riprova";
                }
            }
            return comunicazione?.Replace("&egrave;", "è") ?? "";
        }

        public async Task<DataItemExtended> GetInfoByTrainAsync(List<string> codiceStazionePartenza, List<string> codiceStazioneArrivo, bool isRecursive, string pattern)
        {
            _elencoTreni.Clear();
            var httpClient = _httpClientFactory.CreateClient("Resteasy");
            for (int i = 0; i < codiceStazionePartenza.Count; i++)
            {
                await GetStationsAsync(httpClient, codiceStazionePartenza[i], codiceStazioneArrivo[i], isRecursive, pattern);
            }

            dataList = new DataItemExtended();
            foreach (var treno in _elencoTreni)
            {
                var uri = $@"andamentoTreno/{treno.Item2}/{treno.Item1}/{DateTimeToUnixTimeStamp(DateTime.Today)}";
                var httpResponseMessage = await httpClient.GetAsync(uri);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
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
            }
            httpClient.Dispose();
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
