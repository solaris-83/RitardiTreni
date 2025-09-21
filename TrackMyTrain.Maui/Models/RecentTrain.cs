using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMyTrain.Maui.Models
{
    public partial class RecentTrain : ObservableObject
    {
        public int ID { get; set; }
        public string NumberWithCategory { get; set; }
        public string Number { get; set; }
        public string DepartureStationName { get; set; }
        public string ArrivalStationName { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public int? Ritardo { get; set; }
        public string FormattedDelay { get; set; }
        public bool HasWarning { get; set; }
        [ObservableProperty]
        private bool _isFavorite;

        public RecentTrain()
        {
        }

        public RecentTrain(int id, string numberWithCategory, string number, string departureStationName,
                          string arrivalStationName, string departureTime, string arrivalTime,
                          int? ritardo, string formattedDelay, bool hasWarning, bool isFavorite)
        {
            ID = id;
            NumberWithCategory = numberWithCategory;
            Number = number;
            DepartureStationName = departureStationName;
            ArrivalStationName = arrivalStationName;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Ritardo = ritardo;
            FormattedDelay = formattedDelay;
            HasWarning = hasWarning;
            IsFavorite = isFavorite;
        }
    }
}
