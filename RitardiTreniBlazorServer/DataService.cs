using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataServiceLibrary.Model;
using DataServiceLibrary.Responses;
using RestSharp;

namespace DataServiceLibrary
{
    public class DataService : IDataService
    {
        List<string> _elencoTreni;
        DataItemExtended dataList;

        public DataService()
        {
            _elencoTreni = new List<string>();
            dataList = new DataItemExtended();
        }

        private async Task GetStazioni(string codiceStazionePartenza, string codiceStazioneArrivo, bool isRecursive, string pattern)
        {
            var request = new RestRequest(@"http://www.viaggiatreno.it/viaggiatrenonew/resteasy/viaggiatreno/soluzioniViaggioNew/{codiceStazionePartenza}/{codiceStazioneArrivo}/{dataOra}", Method.GET);

            request.AddUrlSegment("codiceStazionePartenza", codiceStazionePartenza.TrimStart(new char[] { 'S', '0' }));
            request.AddUrlSegment("codiceStazioneArrivo", codiceStazioneArrivo.TrimStart(new char[] { 'S', '0' }));
            request.AddUrlSegment("dataOra", DateTime.Now.ToString("yyyy-MM-ddT00:00:00"));

            var client = new RestClient(request.Resource);
            var response = await client.ExecuteTaskAsync<ElencoNumeriTreni>(request);

            var responseParsed = response.Data;
            responseParsed?.soluzioni?.ForEach(sol => sol.vehicles.ForEach(veh =>
            {
                if ((veh.categoria == "235" && veh.categoriaDescrizione == "RV") || (veh.categoria == "197" && veh.categoriaDescrizione == "Regionale"))
                {
                    if (!_elencoTreni.Contains(veh.numeroTreno) && Regex.IsMatch(veh.numeroTreno, pattern))
                        _elencoTreni.Add(veh.numeroTreno);
                }

            }));

            if (!isRecursive)
            {
                _elencoTreni.Sort();
                return;
            }


            await GetStazioni(codiceStazioneArrivo, codiceStazionePartenza, false, pattern);

            _elencoTreni.Sort();
        }



        public async Task<string> MostraArrivo(string numeroTreno, string nomeStazione, DateTime dataSelezionata)
        {
            string comunicazione = "";
            var requestTreno = new RestRequest(@"resteasy/viaggiatreno/autocompletaStazione/{stazione}", Method.GET);

            requestTreno.RequestFormat = DataFormat.None;
            requestTreno.AddHeader("Accept", "text/plain");
            requestTreno.AddUrlSegment("stazione", nomeStazione);
            var clientTreno = new RestClient("http://www.viaggiatreno.it/viaggiatrenonew");
            var responseTreno = await clientTreno.ExecuteTaskAsync(requestTreno);

            CookieContainer cookiecon = new CookieContainer();

            var responseToParse = responseTreno.Content.ToString().TrimEnd('\r', '\n');
            if (string.IsNullOrEmpty(responseToParse))
                return comunicazione;
            var info_1 = responseToParse.Split('\n')[0].Split('|');
            if (info_1[0] != nomeStazione)
                return comunicazione;
            var info_2 = info_1[1].Split('-');
            var request = new RestRequest("StampaTreno", Method.POST);

            /*request.AddQueryParameter("numTreno", numeroTreno);
            request.AddQueryParameter("locArrivo", info_2[0]);
            request.AddQueryParameter("locArrivoDesc", nomeStazione);
            request.AddQueryParameter("date", dataSelezionata.ToString("dd-MM-yyyy"));
            */
            var response = await clientTreno.ExecuteTaskAsync<List<ComunicazioneArrivo>>(request);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var cookie = response.Cookies.FirstOrDefault(c => c.Name == "JSESSIONID");
                if (cookie != null)
                    cookiecon.Add(new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain));

                clientTreno.CookieContainer = cookiecon;
                var responseParsed = response.Data?.FirstOrDefault();
                comunicazione = responseParsed?.comunicazione;
                if (string.IsNullOrEmpty(comunicazione))
                {
                    request.AddQueryParameter("numTreno", numeroTreno);
                    request.AddQueryParameter("locArrivo", info_2[0]);
                    request.AddQueryParameter("locArrivoDesc", nomeStazione);
                    request.AddQueryParameter("date", dataSelezionata.ToString("dd-MM-yyyy"));
                    response = await clientTreno.ExecuteTaskAsync<List<ComunicazioneArrivo>>(request);
                    responseParsed = response.Data?.FirstOrDefault();
                    comunicazione = responseParsed?.comunicazione;
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

            dataList = new DataItemExtended();
            foreach (var numero in _elencoTreni)
            {
                var requestTreno = new RestRequest(@"http://www.viaggiatreno.it/viaggiatrenonew/resteasy/viaggiatreno/cercaNumeroTrenoTrenoAutocomplete/{numeroTreno}", Method.GET);
                requestTreno.RequestFormat = DataFormat.None;
                requestTreno.AddHeader("Accept", "text/plain");
                requestTreno.AddUrlSegment("numeroTreno", numero);
                var clientTreno = new RestClient(requestTreno.Resource);
                var responseTreno = await clientTreno.ExecuteTaskAsync(requestTreno);
                var responseToParse = responseTreno.Content.ToString().TrimEnd('\r', '\n');
                if (string.IsNullOrEmpty(responseToParse))
                    continue;
                var info_1 = responseToParse.Split('\n')[0].Split('|');
                var info_2 = info_1[1].Split('-');

                var request = new RestRequest(@"http://www.viaggiatreno.it/viaggiatrenonew/resteasy/viaggiatreno/andamentoTreno/{codiceStazione}/{numeroTreno}/{time}", Method.GET);
                request.AddUrlSegment("codiceStazione", info_2[1]);
                request.AddUrlSegment("numeroTreno", info_2[0]);
                request.AddUrlSegment("time", info_2[2]);

                var client = new RestClient(request.Resource);
                var response = await client.ExecuteTaskAsync<List<OrarioTreni>>(request);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseParsed = response.Data;
                    if (responseParsed != null)
                    {

                        foreach (var element in responseParsed)
                        {
                            if (element.fermate != null)
                            {

                                string[] input = new string[3] { element.tipoTreno, element.provvedimento.ToString(), element.subTitle };
                                var statoTreno = GetStatoTreno(input);
                                if (element.fermate.Count > 0)
                                {
                                    element.fermate.ForEach(f =>
                                    {
                                        dataList.DataList.Add(new DataItem(numero, f.stazione, f.ritardo, string.IsNullOrEmpty(statoTreno) ? UnixTimeStampToDateTime(double.Parse(f.programmata.ToString())).ToString("HH:mm") : statoTreno, f.tipoFermata == "A" || f.tipoFermata == "P"));
                                    });
                                }
                                else
                                {
                                    dataList.DataList.Add(new DataItem(numero, "", 0, string.IsNullOrEmpty(statoTreno) ? "" : statoTreno, true));
                                }

                            }
                            dataList.OraUltimoRilevamento = UnixTimeStampToDateTime(double.Parse(element.oraUltimoRilevamento.ToString())).ToString("HH:mm");
                            dataList.StazioneUltimoRilevamente = element.stazioneUltimoRilevamento;
                            dataList.Categoria = element.categoria;
                            dataList.NumeroTreno = element.numeroTreno.ToString();
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

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }
}
