using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Data.Implementations;

namespace TrackMyTrain.Data.Interfaces
{
    public interface IRssReaderService
    {
        Task<IEnumerable<ExtendedFeedItem>> RetrieveAsync(string uri);
    }
}
