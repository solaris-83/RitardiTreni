using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.LocalDb.Models;
using TrackMyTrain.Maui.Models;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class TrainsSearchViewModel : BaseViewModel
    {
        private readonly IHttpDataService _httpDataService;
        private readonly IPopupService _popupService;
        private readonly LocalDbService _dbService;

        public TrainsSearchViewModel(INotificationHandler notificationHandler, IHttpDataService httpDataService, IPopupService popupService, LocalDbService dbService) : base(notificationHandler)
        {
            _httpDataService = httpDataService;
            _popupService = popupService;
            _dbService = dbService;
            LoadDataCommand = new AsyncRelayCommand(async () => await LoadDataAsync());
        }

        [ObservableProperty]
        private string _currentTrainNumber;

        [ObservableProperty]
        private ObservableCollection<RecentTrain> _recentTrains = new ObservableCollection<RecentTrain>();

        public event EventHandler SearchTrainCompleted;

        public override void Appearing()
        {
            base.Appearing();
            _ = LoadDataCommand.ExecuteAsync(default);
        }

        [RelayCommand]
        private async Task LoadTrainDetails()
        {
            await Shell.Current.GoToAsync("traindetails");
        }

        [RelayCommand]
        private void ClearCurrentTrainValue()
        {
            CurrentTrainNumber = string.Empty;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                var results = await _dbService.GetAllAsync<Trains>();
                results = results.OrderByDescending(res => res.IsFavorite).ThenByDescending(res => res.CreatedAt);

                // Recupero i dati necessari dell'andamento del treno e li preparo per mostrarli
                if (results.Any())
                {
                    var itemsToLoad = new List<RecentTrain>();

                    foreach (var train in results)
                    {
                        List<TrainAutocomplete> stationTrains = await _httpDataService.GetTrainsByNumberAsync(train.Number);
                        // Carico solo i treni che circolano oggi
                        if (stationTrains.Any())
                        {
                            TrainAutocomplete autocompleteTrain = stationTrains.SingleOrDefault(tr => tr.TrainNumber == train.Number && tr.DepartureStationShortCode == train.DepartureStationShortCode);
                            if (autocompleteTrain == null)
                            {
                                throw new Exception($"Couldn't find a train matching with number {train.Number} and departure station code {train.DepartureStationShortCode}");
                            }

                            TrainJourney trainJourney = await _httpDataService.GetTrainJourneyAsync(autocompleteTrain);
                            itemsToLoad.Add(new RecentTrain(train.ID, trainJourney.CompNumeroTreno, autocompleteTrain.TrainNumber, autocompleteTrain.DepartureStationName, train.ArrivalStationName, trainJourney.CompOrarioPartenza, trainJourney.CompOrarioArrivo, trainJourney.Ritardo, FormatDelay(trainJourney), trainJourney.HasWarning(), train.IsFavorite));
                        }
                    }

                    RecentTrains = new ObservableCollection<RecentTrain>(itemsToLoad);
                }
                else
                {
                    RecentTrains.Clear();
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
        private async Task DeleteAsync(RecentTrain trainToDelete)
        {
            try
            {
                bool success = await Shell.Current.DisplayAlert("Cancellazione treno", $"Vuoi cancellare il treno {trainToDelete.Number}?", "OK", "Cancella");
                if (success)
                {
                    await _dbService.DeleteItemByKeyAsync<Trains>(trainToDelete.ID);
                    RecentTrains.Remove(trainToDelete);
                }
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
            finally
            {
                SearchTrainCompleted.Invoke(this, EventArgs.Empty);
            }
        }

        [RelayCommand]
        private async Task SetFavoriteAsync(RecentTrain favoriteTrain)
        {
            var train = await _dbService.GetItemByKeyAsync<Trains>(favoriteTrain.ID);
            train.IsFavorite = !train.IsFavorite;
            
            await _dbService.UpdateItemAsync(train);
            favoriteTrain.IsFavorite = train.IsFavorite;
        }

        [RelayCommand]
        private async Task SearchTrainAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentTrainNumber))
                    return;
                TrainAutocomplete autocompleteTrain = default;
                List<TrainAutocomplete> stationTrains = await _httpDataService.GetTrainsByNumberAsync(CurrentTrainNumber);

                if (stationTrains.Count == 0)
                {
                    NotificationHandler.HandleError(new Exception($"Il treno {CurrentTrainNumber} non esiste o non circola oggi."));
                    CurrentTrainNumber = default;
                    return;
                }
                // Se c'è ambiguità lascio scegliere
                if (stationTrains.Count > 1)
                {
                    var queryAttributes = new Dictionary<string, object>
                    {
                        [nameof(TrainsSelectionViewModel.StationTrains)] = stationTrains
                    };

                    var result = await _popupService.ShowPopupAsync<TrainsSelectionViewModel>(Shell.Current, options: new PopupOptions { CanBeDismissedByTappingOutsideOfPopup = true }, queryAttributes);
                     // TODO completare con logica corretta
                    //autocompleteTrain = result 
                }
                else
                {
                    autocompleteTrain = stationTrains.Single();
                }
                // Recupero le info sull'andamento del treno
                TrainJourney trainJourney = await _httpDataService.GetTrainJourneyAsync(autocompleteTrain);
                // Se non ancora presente a DB lo aggiungo
                var existingTrain = await _dbService.GetFirstFilteredAsync<Trains>(train => train.DepartureStationName == autocompleteTrain.DepartureStationName && train.Number == autocompleteTrain.TrainNumber);
                int newID = 0;
                if (existingTrain == null /*|| !existingTrain.Any()*/)
                {
                    var trainToAdd = new Trains()
                    {
                        IsFavorite = false,
                        DepartureStationName = autocompleteTrain.DepartureStationName,
                        DepartureStationShortCode = autocompleteTrain.DepartureStationShortCode,
                        ArrivalStationName = trainJourney.Destinazione, 
                        ArrivalStationShortCode = trainJourney.IdDestinazione,
                        Number = autocompleteTrain.TrainNumber
                    };

                    newID = await _dbService.AddItemAsync(trainToAdd);
                }
                var formattedDelay = FormatDelay(trainJourney);
                // Lo aggiungo alla CollectionView solo se non già presente
                if (!RecentTrains.Any(rec => rec.Number == autocompleteTrain.TrainNumber && rec.DepartureStationName == autocompleteTrain.DepartureStationName))
                {
                    RecentTrains.Insert(0, new RecentTrain(newID, trainJourney.CompNumeroTreno, autocompleteTrain.TrainNumber, autocompleteTrain.DepartureStationName, trainJourney.Destinazione, trainJourney.CompOrarioPartenza, trainJourney.CompOrarioArrivo, trainJourney.Ritardo, formattedDelay, trainJourney.HasWarning(), false));
                    SearchTrainCompleted?.Invoke(this, EventArgs.Empty);
                }

                CurrentTrainNumber = default;
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
        }

        private string FormatDelay(TrainJourney trainJourney)
        {
            if (trainJourney != null)
            {
                if (trainJourney.NonPartito)
                    return string.Empty;
                if (trainJourney.Ritardo == null)
                    return string.Empty;
                if (trainJourney.HasWarning() && trainJourney.Ritardo == 0)
                    return string.Empty;
                return $"{trainJourney.Ritardo}'";
            }
            else 
                return string.Empty;
        }
    }
}
