using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DataServiceLibrary;
using DataServiceLibrary.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
namespace Ritardi_treni.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    /// 
    
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private ObservableCollection<DataItem> _dataItems;

        private string _trattaSelezionata;

        private bool _isBusy;
        private bool _isToggleChecked;
        private string _outputString = "";

        private string _numeroTreno = "";
        private string _nomeStazione = "";
        private DateTime _selectedDate = DateTime.Now;
        private DateTime _dateStart;
        private DateTime _dateEnd;
        private Dictionary<string, string> _dictStation;

        public RelayCommand Command { get; private set; }
        public RelayCommand VisualizzaCmd { get; private set; }

        public ObservableCollection<DataItem> DataItems
        {
            get { return _dataItems; }
            set { Set(ref _dataItems, value); }
        }
        public string TrattaSelezionata
        {
            get
            {
                return _trattaSelezionata;
            }
            set
            {
                Set(ref _trattaSelezionata, value);
            }
        }

        public ICollectionView _customerView;

        public ICollectionView Customers
        {
            get { return _customerView; }
        }

        public string OutputString
        {
            get { return _outputString; }
            set { Set(ref _outputString, value); }
        }

        public string NumeroTreno
        {
            get { return _numeroTreno; }
            set { Set(ref _numeroTreno, value); }
        }

        public string NomeStazione
        {
            get { return _nomeStazione; }
            set { Set(ref _nomeStazione, value); }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { Set(ref _selectedDate, value); }
        }

        public DateTime DateStart
        {
            get { return _dateStart; }
            set { Set(ref _dateStart, value); }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set { Set(ref _dateEnd, value); }
        }

        public bool IsBusy
        {
            get  { return _isBusy; }
            set  { Set(ref _isBusy, value); }
        }
        
        public bool IsToggleChecked
        {
            get { return _isToggleChecked; }
            set { Set(ref _isToggleChecked, value); }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Command = new RelayCommand(async () => await ExecuteCommand(), () => true);
            VisualizzaCmd = new RelayCommand(async () => await VisualizzaCommand(), () => true);
            DateStart = DateTime.Now.AddDays(-7);
            DateEnd = DateTime.Now;
            //PopulateStationDictionary();
        }

        private void PopulateStationDictionary()
        {
            _dictStation = new Dictionary<string, string>();
            _dictStation.Add("BIELLA S.PAOLO", "S00070");
            _dictStation.Add("CARPIGNANO SESIA", "S00971");
            _dictStation.Add("CHIVASSO", "S00232");
            _dictStation.Add("COSSATO", "S00973");
            _dictStation.Add("MAGENTA", "S01040");
            _dictStation.Add("MILANO CENTRALE", "S01700");
            _dictStation.Add("MILANO PORTA GARIBALDI", "S01645");

            _dictStation.Add("NOVARA", "S00248");
            _dictStation.Add("RHO FIERA MILANO", "S01039");
            _dictStation.Add("ROVASENDA", "S00053");
            _dictStation.Add("SALUSSOLA", "S00074");

            _dictStation.Add("SANTHIA`", "S00240");
            _dictStation.Add("TORINO P.NUOVA", "S00219");
            _dictStation.Add("TORINO PORTA SUSA", "S00035");
            _dictStation.Add("VERCELLI", "S00245");
        }

        private async Task VisualizzaCommand()
        {
            IsBusy = true;
            OutputString = string.Empty;
            try
            {
                OutputString = await _dataService.MostraArrivo(NumeroTreno, NomeStazione, SelectedDate);
                IsBusy = false;
            }
            catch(Exception ex)
            {
                IsBusy = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExecuteCommand()
        {
            IsBusy = true;
            DataItems?.Clear();
            var risultati = new DataItemExtended();
            try
            {
                switch (_trattaSelezionata)
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
                _customerView = CollectionViewSource.GetDefaultView(DataItems);
                IsBusy = false;
            }
            catch(Exception ex)
            {
                IsBusy = false;
                MessageBox.Show(ex.Message);
            }
        }
    }
}