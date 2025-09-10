using SimpleFeedReader;
using System.ServiceModel.Syndication;
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;

namespace TrackMyTrain.Data.Services
{
    public class RssReaderService : IRssReaderService
    {
        private readonly IFeedReader _feedReader;
        public RssReaderService(IFeedReader feedReader)
        {
            _feedReader = feedReader;
        }

        public async Task<IEnumerable<ExtendedFeedItem>> RetrieveAsync(string uri)
        {
            var items = await _feedReader.RetrieveFeedAsync(uri);
            IEnumerable<ExtendedFeedItem> newItems = null;
            if (items.Any())
            {
                newItems = items.Select(i => new ExtendedFeedItem(i));
            }
            return newItems;
        }
    }

    public class ExtendedFeedItemNormalizer : DefaultFeedItemNormalizer, IFeedItemNormalizer
    {
        public ExtendedFeedItemNormalizer()
        {

        }
        public override FeedItem Normalize(SyndicationFeed feed, SyndicationItem item) => new ExtendedFeedItem(base.Normalize(feed, item))
        {

        };
    }
}
