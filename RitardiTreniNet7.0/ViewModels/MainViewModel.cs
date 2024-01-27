using CommunityToolkit.Mvvm.ComponentModel;
using DataServiceLibrary;
using DataServiceLibrary.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace RitardiTreniNet7._0
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDataService _dataService;

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

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataItems = new ObservableCollection<DataItem>();
        }

        private async Task ExecuteCommandAsync()
        {
            IsBusy = true;
            DataItems?.Clear();
            var risultati = new DataItemExtended();
            try
            {
                switch (TrattaSelezionata)
                {
                    case "BIELLA - NOVARA":
                        risultati = await _dataService.GetInfoByTrain(new string[] { "S00070" }, new string[] { "S00248" }, true, @"^11[6|7]\d{2}$"); //BI_NO
                        break;
                    case "BIELLA - SANTHIA'":
                        risultati = await _dataService.GetInfoByTrain(new string[] { "S00070" }, new string[] { "S00240" }, true, @"^117\d{2}$");   //BI_SAN
                        break;
                    case "TORINO - MILANO":
                        risultati = await _dataService.GetInfoByTrain(new string[] { "S00219", "S00452", "S00452" }, new string[] { "S01700", "S01645", "S00248" }, true, @"^2\d{3}$");  //TO_MI CENTR
                        break;

                }
                DataItems = new ObservableCollection<DataItem>(risultati.DataList);
              //  _customerView = CollectionViewSource.GetDefaultView(DataItems);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                MessageBox.Show(ex.Message);
            }
        }

        private async Task VisualizzaCommandAsync()
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
