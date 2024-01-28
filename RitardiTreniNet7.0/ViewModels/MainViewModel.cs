using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataServiceLibrary.Model;
using DataServiceLibrary.Services;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RitardiTreniNet7._0
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;
        private readonly IEnumerable<TrainJourneys>? _journeys;

        [ObservableProperty]
        private DateTime? _dateStart;

        [ObservableProperty]
        private DateTime? _dateEnd;

        public List<string>? Stations { get; set; }

        [ObservableProperty]
        private IEnumerable<string>? _trainJourneys;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _trattaSelezionata = "";

        [ObservableProperty]
        private ObservableCollection<DataItem> _dataItems;

        [ObservableProperty]
        private string _outputString = "";

        [ObservableProperty]
        private string _numeroTreno = "";

        [ObservableProperty]
        private string _nomeStazione = "";

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Now;

        public MainViewModel(IDataService dataService, ILogger<MainViewModel> logger)
        {
            _logger = logger;
            _dataService = dataService;
            _dataItems = new ObservableCollection<DataItem>();
            using StreamReader stream1 = new StreamReader(@"Resources\lines.json");
            //_trainLines = JsonSerializer.Deserialize<IEnumerable<TrainLine>>(stream1.ReadToEnd());
            using StreamReader stream2 = new StreamReader(@"Resources\journeys.json");
            _journeys = JsonSerializer.Deserialize<IEnumerable<TrainJourneys>>(stream2.ReadToEnd());
            _trainJourneys = _journeys?.Select(t => t.Name).Distinct();
            LoadStations();
            DateStart = DateTime.Now.AddDays(-7);
            DateEnd = DateTime.Now;
        }

        partial void OnTrattaSelezionataChanged(string value)
        {
            _ = GetInfoAsync();
        }

        [RelayCommand]
        private async Task GetInfoAsync()
        {
            IsBusy = true;
            DataItems?.Clear();
            try
            {
                var p = _journeys?.FirstOrDefault(t => t.Name == TrattaSelezionata);
                if (p != null)
                {
                    var results = await _dataService.GetInfoByTrain(p.StationCodesFrom.ToList(), p.StationCodesTo.ToList(), true, p.Pattern);
                    DataItems = new ObservableCollection<DataItem>(results.DataList);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                _logger.LogError(ex, "GetInfoAsync");
                MessageBox.Show(ex.Message);
            }
        }

        [RelayCommand]
        private async Task ShowArrivalsAsync()
        {
            IsBusy = true;
            OutputString = string.Empty;
            try
            {
                OutputString = await _dataService.MostraArrivo(NumeroTreno, NomeStazione, SelectedDate);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                _logger.LogError(ex, "GetInfoAsync");
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStations()
        {
            Stations ??= new List<string>();

            Stations.Add("BIELLA S.PAOLO");
            Stations.Add("CARPIGNANO SESIA");
            Stations.Add("CHIVASSO");
            Stations.Add("COSSATO");
            Stations.Add("MAGENTA");
            Stations.Add("MILANO CENTRALE");
            Stations.Add("MILANO PORTA GARIBALDI");
            Stations.Add("NOVARA");
            Stations.Add("RHO FIERA MILANO");
            Stations.Add("ROVASENDA");
            Stations.Add("SALUSSOLA");
            Stations.Add("SANTHIA`");
            Stations.Add("TORINO P.NUOVA");
            Stations.Add("TORINO PORTA SUSA");
            Stations.Add("VERCELLI");
        }
    }
}
