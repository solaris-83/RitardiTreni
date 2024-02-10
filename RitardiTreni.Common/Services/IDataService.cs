
using RitardiTreni.Common.Model;

namespace RitardiTreni.Common.Services
{
    public interface IDataService
    {
        Task<DataItemExtended> GetInfoByTrainAsync(List<string> codiceStazionePartenza, List<string> codiceStazioneArrivo, string pattern);
        Task<string> ShowArrivalsAsync(string numeroTreno, string nomeStazione, DateTime dataSelezionata);
    }
}
