using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class CustomPopupViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IPopupService _popupService;

        [ObservableProperty]
        private string _name;

        public CustomPopupViewModel(IPopupService popupService)
        {
            _popupService = popupService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Name = (string)query[nameof(CustomPopupViewModel.Name)];
        }

        

        [RelayCommand]
        async Task Close()
        {
            var result = await _popupService.ClosePopupAsync(Shell.Current);
        }
    }
}
