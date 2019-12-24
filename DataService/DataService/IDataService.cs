using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceLibrary.Model;

namespace DataServiceLibrary
{
    public interface IDataService
    {
        Task<DataItemExtended> GetInfoByTrain(string[] codiceStazionePartenza, string[] codiceStazioneArrivo, bool isRecursive, string pattern);
        Task<string> MostraArrivo(string numeroTreno, string nomeStazione, DateTime dataSelezionata);
    }
}
