using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Models;
using RitardiTreni.Common.Model;
using RitardiTreni.Common.Services;
using RitardiTreniNet7.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace RitardiTreniNet7
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly ILogger _logger;
        private readonly IEnumerable<TrainJourneys>? _journeys;
        private readonly IConfiguration _configuration;
        private readonly IDialogService _dialogService;

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
        [NotifyCanExecuteChangedFor(nameof(ShowArrivalsCommand))]
        private string _numeroTreno = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ShowArrivalsCommand))]
        private string _nomeStazione = "";

        [ObservableProperty]
        private DateTime _selectedDate = DateTime.Now;

        public MainViewModel(IDataService dataService, ILogger<MainViewModel> logger, IConfiguration configuration, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _logger = logger;
            _configuration = configuration;
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
            ShowArrivalsCommand.NotifyCanExecuteChanged();
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
                    var results = await _dataService.GetInfoByTrainAsync(p.StationCodesFrom.ToList(), p.StationCodesTo.ToList(), true, p.Pattern);
                    DataItems = new ObservableCollection<DataItem>(results.DataList);
                }
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                _logger.LogError(ex, "GetInfoAsync");
                _dialogService.ShowDialog<MessageNotificationViewModel>(new DialogParameters
                {
                    { "Message", ex.Message }
                });
            }
        }

        private bool CanShowArrivals()
        {
            return !string.IsNullOrWhiteSpace(NumeroTreno) && !string.IsNullOrWhiteSpace(NomeStazione);
        }

        [RelayCommand(CanExecute = nameof(CanShowArrivals))]
        private async Task ShowArrivalsAsync()
        {
            IsBusy = true;
            OutputString = string.Empty;
            try
            {
                OutputString = await _dataService.ShowArrivalsAsync(NumeroTreno, NomeStazione, SelectedDate);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                _logger.LogError(ex, "ShowArrivalsAsync");
                _dialogService.ShowDialog<MessageNotificationViewModel>(new DialogParameters
                {
                    { "Message", ex.Message }
                });
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
