using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Data.Implementations;

namespace TrackMyTrain.Maui.Models
{
    public partial class RecentTrain : ObservableObject
    {
        public int ID { get; private set; }
        public string NumberWithCategory { get; private set; }
        public string Number { get; private set; }
        public string DepartureStationName { get; private set; }
        public string ArrivalStationName { get; private set; }
        public string DepartureTime { get; private set; }
        public string ArrivalTime { get; private set; }
        public int? Ritardo { get; private set; }
        public bool HasWarning { get; private set; }
        public string FormattedDelay { get; private set; }
        public string SubTitle { get; private set; }
        public string StazioneUltimoRilevamento { get; private set; }
        public string CompOraUltimoRilevamento { get; private set; }
        public bool HasWarningAndFormattedDelayIsNotEmpty => HasWarning && !string.IsNullOrEmpty(FormattedDelay);
        public bool? NonPartito { get; private set; }
        public bool ShowInfoTrain => CompOraUltimoRilevamento != null && CompOraUltimoRilevamento != "--"; // !(NonPartito && CompOraUltimoRilevamento == "--" && string.IsNullOrWhiteSpace(SubTitle) && StazioneUltimoRilevamento == "--");


        [ObservableProperty]
        private bool _isFavorite;
        
        public RecentTrain()
        {
        }

        public RecentTrain(int id, string numberWithCategory, string number, string departureStationName,
                          string arrivalStationName, string departureTime, string arrivalTime,
                          int? ritardo, string formattedDelay, bool hasWarning, bool isFavorite, string subTitle, string stazioneUltimoRilevamento, string compOraUltimoRilevamento, bool nonPartito)
        {
            ID = id;
            NumberWithCategory = numberWithCategory;
            Number = number;
            DepartureStationName = departureStationName;
            ArrivalStationName = arrivalStationName;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Ritardo = ritardo;
            HasWarning = hasWarning;
            IsFavorite = isFavorite;
            FormattedDelay = formattedDelay;
            SubTitle = subTitle;
            CompOraUltimoRilevamento = compOraUltimoRilevamento;
            StazioneUltimoRilevamento = stazioneUltimoRilevamento;
            NonPartito = nonPartito;
        }
    }

    public class RecentTrainGroup : ObservableCollection<RecentTrain>
    {
        public bool IsFavorite { get; private set; }
       // public ObservableCollection<RecentTrain> Values { get; private set; }
       
        public RecentTrainGroup(bool isFavorite, IEnumerable<RecentTrain> recentTrains) : base(recentTrains)
        {
            IsFavorite = isFavorite;
           // Values = recentTrains;
        }
    }
}
