
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using TrackMyTrain.Maui.Models;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class CustomPopupViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IPopupService _popupService;

        [ObservableProperty]
        private LastTrackedTrain _information;

        public CustomPopupViewModel(IPopupService popupService)
        {
            _popupService = popupService;
        }

        //public async Task OnPopupNavigatedAsync(IReadOnlyDictionary<string, object?> parameters)
        //{
        //    if (parameters["RecentTrain"] is RecentTrain recentTrain)
        //    {
        //        Information = new LastTrackedTrain()
        //        {
        //            SubTitle = recentTrain.SubTitle,
        //            CompOraUltimoRilevamento = recentTrain.CompOraUltimoRilevamento,
        //            StazioneUltimoRilevamento = recentTrain.StazioneUltimoRilevamento
        //        };
        //    }
        //}

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["RecentTrain"] is RecentTrain recentTrain)
            {
                Information = new LastTrackedTrain()
                {
                    SubTitle = recentTrain.SubTitle,
                    CompOraUltimoRilevamento = recentTrain.CompOraUltimoRilevamento,
                    StazioneUltimoRilevamento = recentTrain.StazioneUltimoRilevamento
                };
            }
        }
    }
}
