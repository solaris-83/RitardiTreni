using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class RssFeedViewModel : BaseViewModel
    {
        private readonly IRssReaderService _feedReaderService;
        private readonly IPopupService _popupService;
        private string _selectedCategory = string.Empty;
        public List<string> Categories { get; private set; } = new List<string>();

        public RssFeedViewModel(IRssReaderService feedReaderService, INotificationHandler notificationHandler, IPopupService popupService) : base(notificationHandler)
        {
            _feedReaderService = feedReaderService;
            _popupService = popupService;
            LoadSectors();
            LoadDataCommand = new AsyncRelayCommand(async () => await LoadFeedItems());
            _selectedCategory = "Ferroviario";
        }

        private void LoadSectors()
        {
            Categories.Add("Aereo");
            Categories.Add("Appalti ferroviari");
            Categories.Add("Circolazione e sicurezza stradale");
            Categories.Add("Elicotteri");
            Categories.Add("Ferroviario");
            Categories.Add("Generale");
            Categories.Add("Marittimo");
            Categories.Add("Ncc");
            Categories.Add("Plurisettoriale");
            Categories.Add("Taxi");
            Categories.Add("Trasporto merci");
            Categories.Add("Trasporto pubblico locale");
        }

        [ObservableProperty]
        private ObservableCollection<StrikeGroup> _strikesGrouped;

        private async Task LoadFeedItems()
        {
            try
            {
               // IsRefreshing = true;
                StrikesGrouped?.Clear();
                var feeds = await _feedReaderService.RetrieveAsync("https://scioperi.mit.gov.it/mit2/public/scioperi/rss");
                var feedsgrouped = feeds.Where(feed => feed.Settore == _selectedCategory).ToList().GroupBy(feed => $"{feed.DataInizio} - {feed.DataFine}")
                    .Select(g =>
                    new StrikeGroup(g.Key, g.Select(strike => new Strike { Title = strike.CategoriaInteressata, Area = strike.Rilevanza + $" (Regione: {strike.Regione})", Hours = strike.Modalita, Notes = strike.Sindacati })));

                StrikesGrouped = new ObservableCollection<StrikeGroup>(feedsgrouped);
                if (StrikesGrouped.Count == 0)
                {
                    await Shell.Current.DisplayAlert($"Settore {_selectedCategory}", "Nessuno sciopero previsto", "Chiudi");
                }
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private async Task OpenPopupAsync()
        {
            var queryAttributes = new Dictionary<string, object>();
            queryAttributes.Add("Categories", Categories);
            var returningValue = await _popupService.ShowPopupAsync<CategoriesPopupViewModel>(Shell.Current, null, queryAttributes);
            if (returningValue != null && returningValue is IPopupResult<string> popupResult)
            {

                _selectedCategory = popupResult.WasDismissedByTappingOutsideOfPopup ? string.Empty : popupResult.Result;
            }
        }

        

        public override async void Appearing()
        {
            base.Appearing();
            _ = LoadDataCommand.ExecuteAsync(default);
        }
    }

    public class StrikeGroup : ObservableCollection<Strike>
    {
        public string Date { get; set; }

        public StrikeGroup(string date, IEnumerable<Strike> strikes) : base(strikes)
        {
            Date = date;
        }
    }

    public class Strike
    {
        public string Title { get; set; }
        public string Area { get; set; }
        public string Hours { get; set; }
        public string Notes { get; set; }
    }
}
