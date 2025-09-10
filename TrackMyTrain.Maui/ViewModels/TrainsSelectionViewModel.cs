using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Data.Implementations;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class TrainsSelectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<TrainAutocomplete> _stationTrains;
    }
}
