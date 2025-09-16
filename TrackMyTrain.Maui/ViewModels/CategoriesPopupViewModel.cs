
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class CategoriesPopupViewModel : BaseViewModel, IQueryAttributable
    {
        private readonly IPopupService _popupService;
        private readonly INavigation navigation = Application.Current.Windows[0].Page.Navigation ?? throw new NotImplementedException($"Couldn't get Navigation for {Application.Current.Windows[0].Page}");

        [ObservableProperty]
        private ObservableCollection<string> _categories;

        public CategoriesPopupViewModel(INotificationHandler notificationHandler, IPopupService popupService) : base(notificationHandler)
        {
            _popupService = popupService;
            Categories = new ObservableCollection<string>();
        }

        [RelayCommand]
        private async Task CloseAsync(string category)
        {
            await _popupService.ClosePopupAsync(navigation, category ?? string.Empty, CancellationToken.None);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query["Categories"] is IEnumerable<string> collection)
            {
                Categories = new ObservableCollection<string>(collection);
            }
        }
    }
}