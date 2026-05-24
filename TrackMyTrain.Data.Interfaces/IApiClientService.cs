

namespace TrackMyTrain.Data.Interfaces
{
    public interface IApiClientService
    {
        string ClientName { get; set; }
        Task<TOut> GetAsync<TOut, TIn>(string url, TIn q, bool authRequired = true, bool isPlainText = false, bool retry = true, CancellationToken? cancellationToken = null) where TOut : class where TIn : class;
        Task<TOut> PostAsync<TOut, TIn>(string url, TIn body, bool authRequired = true, bool retry = true) where TOut : class where TIn : class;
    }
}
