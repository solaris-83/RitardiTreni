
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INotificationHandler NotificationHandler;
        public BaseViewModel(INotificationHandler notificationHandler)
        {
            NotificationHandler = notificationHandler;
        }

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private bool _isRefreshing;

        public IAsyncRelayCommand LoadDataCommand { get; set; }

        public virtual void Appearing() { }
        public virtual void Disappearing() { }
    }

    public class Footer
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }


        public Footer(string message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}
