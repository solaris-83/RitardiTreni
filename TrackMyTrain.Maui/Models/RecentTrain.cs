using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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
        public bool HasWarningAndFormattedDelayIsNotEmpty => HasWarning && !string.IsNullOrEmpty(FormattedDelay);

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
            HasWarning = hasWarning;
            IsFavorite = isFavorite;
            FormattedDelay = formattedDelay;
        }

        //public string FormatDelay(TrainJourney trainJourney)
        //{
        //    if (trainJourney != null)
        //    {
        //        if (trainJourney.NonPartito)
        //            return string.Empty;
        //        if (trainJourney.Ritardo == null)
        //            return string.Empty;
        //        if (trainJourney.HasWarning() && trainJourney.Ritardo == 0)
        //            return string.Empty;
        //        return $"{trainJourney.Ritardo}'";
        //    }
        //    else
        //        return string.Empty;
        //}
    }
}
