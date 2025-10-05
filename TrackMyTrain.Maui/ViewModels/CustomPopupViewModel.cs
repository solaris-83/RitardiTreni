
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using TrackMyTrain.Maui.Models;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class CustomPopupViewModel : ObservableObject//, IQueryAttributable
    {
        private readonly IPopupService _popupService;

        [ObservableProperty]
        private Tuple<string, string, string> _information;

        public CustomPopupViewModel()
        {
        }

        //public void ApplyQueryAttributes(IDictionary<string, object> query)
        //{
        //    if (query["RecentTrain"] is RecentTrain recentTrain)
        //    {
        //        Information = new Tuple<string, string, string>(recentTrain.SubTitle, recentTrain.CompOraUltimoRilevamento, recentTrain.StazioneUltimoRilevamento);
        //    }
        //}
    }
}
