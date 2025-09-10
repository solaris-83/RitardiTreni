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
        private bool _firstRun = true;
        //[ObservableProperty]
        //private bool _isRefreshing;
        public List<string> Categories { get; private set; } = new List<string>();

        [ObservableProperty]
        private string _selectedCategory = "Ferroviario";
        public RssFeedViewModel(IRssReaderService feedReaderService, INotificationHandler notificationHandler) : base(notificationHandler)
        {
            _feedReaderService = feedReaderService;
            LoadDataCommand = new AsyncRelayCommand(async () => await LoadFeedItems());
            LoadSectors();

        }

        [ObservableProperty]
        private ObservableCollection<StrikeGroup> _strikesGrouped;

        partial void OnSelectedCategoryChanged(string? oldValue, string newValue)
        {
            _ = LoadDataCommand.ExecuteAsync(default);
        }

        private async Task LoadFeedItems()
        {
            try
            {
               // IsRefreshing = true;
                StrikesGrouped?.Clear();
                var feeds = await _feedReaderService.RetrieveAsync("https://scioperi.mit.gov.it/mit2/public/scioperi/rss");
                var feedsgrouped = feeds.Where(feed => feed.Settore == SelectedCategory).ToList().GroupBy(feed => $"{feed.DataInizio} - {feed.DataFine}")
                    .Select(g =>
                    new StrikeGroup(g.Key, g.Select(strike => new Strike { Title = strike.CategoriaInteressata, Area = strike.Rilevanza + $" (Regione: {strike.Regione})", Hours = strike.Modalita, Notes = strike.Sindacati })));

                StrikesGrouped = new ObservableCollection<StrikeGroup>(feedsgrouped);
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
            finally
            {
                IsRefreshing = false;
                _firstRun = false;
            }
        //    finally
        //    {
        //        StrikesGrouped = new ObservableCollection<StrikeGroup>
        //{
        //    new StrikeGroup("4 settembre - 5 settembre", new[]
        //    {
        //        new Strike {
        //            Title = "PERSONALE DI MACCHINA E DI BORDO DEL GRUPPO FERROVIE DELLO STATO ITALIANE",
        //            Area = "Area Nazionale",
        //            Hours = "21 ORE: DALLE 21.00 DEL 4/9 ALLE 18.00 DEL 5/9",
        //            Notes = "ASSEMBLEA NAZIONALE PDM/PDB GRUPPO FSI"
        //        }
        //    }),
        //    new StrikeGroup("5 settembre", new[]
        //    {
        //        new Strike {
        //            Title = "PERSONALE SOC. TRENITALIA APPARTENENTE ALLA DOR ( DIREZIONE OPERAZIONI E RETE ) TRENTINO ALTO ADIGE",
        //            Area = "Area Trentino-Alto Adige",
        //            Hours = "8 ORE: DALLE 09.01 ALLE 16.59",
        //            Notes = "OSR UILT-UIL/SLM FAST-CONFSAL/ORSA"
        //        }
        //    }),
        //    new StrikeGroup("15 settembre", new[]
        //    {
        //        new Strike {
        //            Title = "PERSONALE SOC. TRENITALIA PRODUZIONE REGIONALE, PRODUZIONE IC, IMC CARROZZE SERVIZIO INTERCITY REGIONE CALABRIA",
        //            Area = "Area Calabria",
        //            Hours = "8 ORE: DALLE 09.01 ALLE 17.00",
        //            Notes = "OSR UGL FERROVIERI"
        //        }
        //    })
        //};
        //    }
        }

        private void LoadSectors()
        {
            Categories.Add("Generale");
            Categories.Add("Plurisettoriale");
            Categories.Add("Ferroviario");
            Categories.Add("Appalti ferroviari");
            Categories.Add("Trasporto pubblico locale");
            Categories.Add("Marittimo");
            Categories.Add("Trasporto merci");
            Categories.Add("Elicotteri");
            Categories.Add("Taxi");
            Categories.Add("Ncc");
            Categories.Add("Circolazione e sicurezza stradale");
            Categories.Add("Aereo");
            Categories = Categories.OrderBy(s => s).ToList();
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
