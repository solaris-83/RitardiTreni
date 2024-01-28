using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceLibrary.Model;

namespace DataServiceLibrary.Services
{
    public interface IDataService
    {
        Task<DataItemExtended> GetInfoByTrain(List<string> codiceStazionePartenza, List<string> codiceStazioneArrivo, bool isRecursive, string pattern);
        Task<string> MostraArrivo(string numeroTreno, string nomeStazione, DateTime dataSelezionata);
    }
}
