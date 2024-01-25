using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataServiceLibrary.Model;
using DataServiceLibrary.Responses;
using RestSharp;

namespace DataServiceLibrary
{
    public class DataService : IDataService
    {
        List<(string, string)> _elencoTreni;
        DataItemExtended dataList;
        private const string BASE_PATH_1 = BASE_PATH + "/resteasy/viaggiatreno";
        private const string BASE_PATH = "http://www.viaggiatreno.it/infomobilita";
        public DataService()
        {
            _elencoTreni = new List<(string, string)>();
            dataList = new DataItemExtended();
        }

        private async Task GetStazioni(string codiceStazionePartenza, string codiceStazioneArrivo, bool isRecursive, string pattern)
        {
            var client = new RestClient(BASE_PATH_1);
            var request = new RestRequest(@"soluzioniViaggioNew/{codiceStazionePartenza}/{codiceStazioneArrivo}/{dataOra}");

            request.AddUrlSegment("codiceStazionePartenza", codiceStazionePartenza.TrimStart(new char[] { 'S', '0' }));
            request.AddUrlSegment("codiceStazioneArrivo", codiceStazioneArrivo.TrimStart(new char[] { 'S', '0' }));
            request.AddUrlSegment("dataOra", DateTime.Now.ToString("yyyy-MM-ddT00:00:00"));
            var response = await client.GetAsync<ElencoNumeriTreni>(request);
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
            await GetStazioni(codiceStazioneArrivo, codiceStazionePartenza, false, pattern);
            _elencoTreni.OrderBy(s => s.Item1);
        }


        public async Task<string> MostraArrivo(string numeroTreno, string nomeStazione, DateTime dataSelezionata)
        {
            string comunicazione = "";
            var clientTreno = new RestClient(BASE_PATH_1);
            var requestTreno = new RestRequest(@"autocompletaStazione/{stazione}")
            {
                RequestFormat = DataFormat.None
            };
            requestTreno.AddHeader("Accept", "text/plain");
            requestTreno.AddUrlSegment("stazione", nomeStazione.Length > 20 ? nomeStazione.Substring(0, 20) : nomeStazione);
            
            var responseTreno = await clientTreno.GetAsync(requestTreno);
            var responseToParse = responseTreno.Content.ToString().TrimEnd('\r', '\n');
            if (string.IsNullOrEmpty(responseToParse))
                return comunicazione;
            var info_1 = responseToParse.Split('\n')[0].Split('|');
            if (info_1[0] != nomeStazione)
                return comunicazione;
            var info_2 = info_1[1].Split('-');
             clientTreno = new RestClient(BASE_PATH);
            var request = new RestRequest("StampaTreno");

            request.AddQueryParameter("numTreno", numeroTreno);
            request.AddQueryParameter("locArrivo", info_2[0]);
            request.AddQueryParameter("locArrivoDesc", nomeStazione);
            request.AddQueryParameter("date", dataSelezionata.ToString("dd-MM-yyyy"));
            
            var response = await clientTreno.PostAsync(request);
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
                    var response1 = await clientTreno.PostAsync<ComunicazioneArrivo>(request);
                    comunicazione = response1?.comunicazione;
                }
            }
            else
                comunicazione = "Riprova";
            return comunicazione?.Replace("&egrave;", "è");
        }

        public async Task<DataItemExtended> GetInfoByTrain(string[] codiceStazionePartenza, string[] codiceStazioneArrivo, bool isRecursive, string pattern)
        {
            _elencoTreni.Clear();
            for (int i = 0; i < codiceStazionePartenza.Length; i++)
            {
                await GetStazioni(codiceStazionePartenza[i], codiceStazioneArrivo[i], isRecursive, pattern);
            }

            var clientTreno = new RestClient(BASE_PATH_1);
            dataList = new DataItemExtended();
            foreach (var treno in _elencoTreni)
            {
                var request = new RestRequest(@"andamentoTreno/{codiceStazione}/{numeroTreno}/{time}");
                request.AddUrlSegment("codiceStazione", treno.Item2);
                request.AddUrlSegment("numeroTreno", treno.Item1);
                request.AddUrlSegment("time", DateTimeToUnixTimeStamp(DateTime.Today));
                
                var response = await clientTreno.GetAsync<OrarioTreni>(request);
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
