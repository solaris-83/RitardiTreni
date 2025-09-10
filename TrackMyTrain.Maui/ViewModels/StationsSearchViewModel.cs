
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class StationsSearchViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        public StationsSearchViewModel(INotificationHandler notificationHandler, IDataService dataService) : base(notificationHandler)
        {
            _dataService = dataService;
        }

        public override void Appearing()
        {
            base.Appearing();
            if (_dataService.CachedStations.Count == 0)
            {
                NotificationHandler.HandleError(new Exception("Stations data not loaded."));
            }
        }
    }
}
