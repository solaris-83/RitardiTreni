
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.LocalDb.Models;
using TrackMyTrain.Maui.Models;
using TrackMyTrain.Maui.Services;
using TrackMyTrain.Maui.Utilities;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class TrainsSearchViewModel : BaseViewModel
    {
        private readonly IHttpDataService _httpDataService;
        private readonly IPopupService _popupService;
        private readonly LocalDbService _dbService;
        private readonly ILogger<TrainsSearchViewModel> _logger;
        private const byte MAX_NUMBER_OF_FAVORITE_TRAINS = 15;
        private const byte MAX_NUMBER_OF_SEARCHED_TRAINS = 5;

        private bool _canRefresh = true;
        public TrainsSearchViewModel(ILogger<TrainsSearchViewModel> logger, INotificationHandler notificationHandler, IHttpDataService httpDataService, IPopupService popupService, LocalDbService dbService) : base(notificationHandler)
        {
            _httpDataService = httpDataService;
            _popupService = popupService;
            _dbService = dbService;
            _logger = logger;
            LoadDataCommand = new AsyncRelayCommand(async () => await LoadDataAsync());
        }

        [ObservableProperty]
        private string _currentTrainNumber;

        [ObservableProperty]
        private ObservableCollection<RecentTrainGroup> _recentTrains = new ObservableCollection<RecentTrainGroup>();

        public event EventHandler<int> SearchTrainCompleted;

        public override void Appearing()
        {
            base.Appearing();
            if (_canRefresh)
                _ = LoadDataCommand.ExecuteAsync(default);
            _canRefresh = true;
        }

        [RelayCommand]
        private void ShowLastTrackedPosition(RecentTrain recentTrain)
        {
            var queryAttributes = new Dictionary<string, object>
            {
                { "RecentTrain", recentTrain }
            };
            _canRefresh = false;
            _popupService.ShowPopup<CustomPopupViewModel>(Shell.Current, new PopupOptions() { CanBeDismissedByTappingOutsideOfPopup = true }, queryAttributes);
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
                var allTrains = await _dbService.GetAllAsync<Trains>();

                var favorites = allTrains
                    .Where(t => t.IsFavorite);

                var topNonFavorites = allTrains
                    .Where(t => !t.IsFavorite)
                    .OrderByDescending(t => t.LastUpdatedAt).Take(MAX_NUMBER_OF_SEARCHED_TRAINS);

              //  var topNonFavorites = nonFavorites.Take(MAX_NUMBER_OF_SEARCHED_TRAINS);
                var otherNonFavorites = allTrains
                    .Where(t => !t.IsFavorite)
                    .OrderByDescending(t => t.LastUpdatedAt).Skip(MAX_NUMBER_OF_SEARCHED_TRAINS);
                await _dbService.DeleteItemsAsync(otherNonFavorites);

                var results = favorites
                    .Concat(topNonFavorites)
                    .OrderByDescending(t => t.LastUpdatedAt);

                var errors = new List<Exception>();

                var tasks = results.Select(async train =>
                {
                    try
                    {
                        return await MapToRecentTrainAsync(train);
                    }
                    catch (Exception ex)
                    {
                        lock (errors) // thread-safe add
                        {
                            errors.Add(ex);
                        }
                        return null; // skip this one
                    }
                });

                var taskCompleted = await Task.WhenAll(tasks);

                var itemsToLoadGrouped = taskCompleted
                   // .Where(x => x != null)               // filter out nulls
                    .Cast<RecentTrain>()                 // cast safely since nulls are gone
                    .GroupBy(train => train.IsFavorite)  // group by IsFavorite
                    .Select(g => new RecentTrainGroup(g.Key, g)) // pass group to ctor
                    .ToList();

                RecentTrains = new ObservableCollection<RecentTrainGroup>(itemsToLoadGrouped);

                // Show just one error popup if there were failures
                if (errors.Count != 0)
                {
                    NotificationHandler.HandleError(errors.First());
                }
            }
            catch (Exception ex)
            {
                // Handles unexpected things like DB failure
                NotificationHandler.HandleError(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        
        private async Task<RecentTrain?> MapToRecentTrainAsync(Trains train)
        {
            var autocompleteTrain = new TrainAutocomplete(
                train.DepartureStationName,
                train.Number,
                train.DepartureStationShortCode,
                new DateTimeOffset(DateTime.Today).ToUnixTimeMilliseconds());

            var trainJourney = await _httpDataService.GetTrainJourneyAsync(autocompleteTrain);
            if (trainJourney == null) return null;

            var mappedTrain = new RecentTrain(
                train.ID,
                trainJourney.CompNumeroTreno,
                autocompleteTrain.TrainNumber,
                autocompleteTrain.DepartureStationName,
                train.ArrivalStationName,
                trainJourney.CompOrarioPartenza,
                trainJourney.CompOrarioArrivo,
                trainJourney.Ritardo,
                trainJourney.FormatDelay(),
                trainJourney.HasWarning(),
                train.IsFavorite,
                trainJourney.SubTitle,
                trainJourney.compOraUltimoRilevamento,
                trainJourney.StazioneUltimoRilevamento, 
                trainJourney.NonPartito);

            mappedTrain.PropertyChanged += Train_PropertyChanged;
            return mappedTrain;
        }

        private void MoveTrainBetweenGroups(RecentTrain train)
        {
            // Rimuovo dal vecchio gruppo
            var oldGroup = RecentTrains.FirstOrDefault(g => g.Any(t => t == train));
            if (oldGroup != null)
            {
                oldGroup.Remove(train);
                if (oldGroup.Count == 0)
                    RecentTrains.Remove(oldGroup);
            }

            // Aggiungo al nuovo gruppo
            var newGroup = RecentTrains.FirstOrDefault(g => g.IsFavorite == train.IsFavorite);
            if (newGroup == null)
            {
                newGroup = new RecentTrainGroup(train.IsFavorite, new ObservableCollection<RecentTrain>() { train });
                RecentTrains.Add(newGroup);
            }
            else
            {
                newGroup.Add(train);
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
                    var group = RecentTrains.FirstOrDefault(g => g.IsFavorite == trainToDelete.IsFavorite);
                    if (group != null)
                    {
                        trainToDelete.PropertyChanged -= Train_PropertyChanged;
                        group.Remove(trainToDelete); // remove from inner collection
                        if (group.Count == 0)
                            RecentTrains.Remove(group);
                        SearchTrainCompleted.Invoke(this, RecentTrains.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
        }

        [RelayCommand]
        private async Task SetFavoriteAsync(RecentTrain favoriteTrain)
        {
            // Non è possibile aggiungere oltre 15 preferiti
            var allTrains = await _dbService.GetAllAsync<Trains>();
            var numberOfFavoriteTrains = allTrains.Count(t => t.IsFavorite);
            if (numberOfFavoriteTrains >= MAX_NUMBER_OF_FAVORITE_TRAINS && favoriteTrain.IsFavorite == false)
            {
                NotificationHandler.HandleError(new Exception($"Raggiunto il limite massimo di {MAX_NUMBER_OF_FAVORITE_TRAINS} preferiti. Rimuovere un treno dai preferiti per proseguire."));
                return;
            }
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
                // Se non ancora presente a DB lo aggiungo
                var existingTrain = await _dbService.GetFirstFilteredAsync<Trains>(train => train.DepartureStationName == autocompleteTrain.DepartureStationName && train.Number == autocompleteTrain.TrainNumber);
                if (existingTrain == null)
                {
                    // Recupero le info sull'andamento del treno
                    TrainJourney trainJourney = await _httpDataService.GetTrainJourneyAsync(autocompleteTrain);
                    var trainToAdd = new Trains()
                    {
                        IsFavorite = false,
                        DepartureStationName = autocompleteTrain.DepartureStationName,
                        DepartureStationShortCode = autocompleteTrain.DepartureStationShortCode,
                        ArrivalStationName = trainJourney.Destinazione,
                        ArrivalStationShortCode = trainJourney.IdDestinazione,
                        Number = autocompleteTrain.TrainNumber,
                        NumberWithCategory = trainJourney.Categoria,
                        DepartureTime = trainJourney.CompOrarioPartenza,
                        ArrivalTime = trainJourney.CompOrarioArrivo
                    };

                    var newID = await _dbService.AddItemAsync(trainToAdd);
                    var newTrain = new RecentTrain(newID, trainJourney.CompNumeroTreno, autocompleteTrain.TrainNumber, autocompleteTrain.DepartureStationName, trainJourney.Destinazione, trainJourney.CompOrarioPartenza, trainJourney.CompOrarioArrivo, trainJourney.Ritardo, trainJourney.FormatDelay(), trainJourney.HasWarning(), false, trainJourney.SubTitle,
                        trainJourney.compOraUltimoRilevamento,
                        trainJourney.StazioneUltimoRilevamento, trainJourney.NonPartito);

                    newTrain.PropertyChanged += Train_PropertyChanged;
                    var notFavoriteGroup = RecentTrains.FirstOrDefault(group => !group.IsFavorite);
                    if (notFavoriteGroup != null)
                    {
                        notFavoriteGroup.Insert(0, newTrain);
                    }
                    else
                    {
                        //trainToAdd.IsFavorite è necessariamente false qui
                        RecentTrains.Add(new RecentTrainGroup(trainToAdd.IsFavorite, new[] { newTrain }));
                    }
                    // Mantieni un rolling di 5 treni non preferiti
                    var otherNonFavorites = await _dbService.GetFilteredAsync<Trains>(t => !t.IsFavorite);
                    var toDelete = otherNonFavorites.OrderByDescending(t => t.LastUpdatedAt).Skip(MAX_NUMBER_OF_SEARCHED_TRAINS);
                    await _dbService.DeleteItemsAsync(toDelete);
                    foreach (var item in toDelete)
                    {
                        var train = RecentTrains.DeleteTrain(item.ID);
                    }
                }
                else
                {
                    NotificationHandler.HandleError(new Exception($"Il treno {CurrentTrainNumber} è già presente nella lista"));
                }

                 CurrentTrainNumber = default;
            }
            catch (Exception ex)
            {
                NotificationHandler.HandleError(ex);
            }
        }

        public void Train_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RecentTrain.IsFavorite) && sender is RecentTrain train)
            {
                _logger.LogInformation("Called Train_PropertyChanged");
                try
                {
                    MoveTrainBetweenGroups(train);
                }
                catch (Exception ex)
                {
                    NotificationHandler.HandleError(ex);
                }
            }
        }
    }
}
