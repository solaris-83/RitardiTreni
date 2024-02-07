using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Models;
using System.Windows.Input;

namespace RitardiTreniNet7.ViewModels
{
    internal partial class MessageNotificationViewModel : ObservableObject, IDialogViewModel
    {
        private readonly IDialogService _dialogService;

        public MessageNotificationViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            OkCommand = new RelayCommand(Ok);
        }

        private void Ok()
        {
            _dialogService.SetReturnParameters(true);
            _dialogService.CloseDialog<MessageNotificationViewModel>();
        }

        public ICommand OkCommand { get; set; }

        [ObservableProperty]
        private string? _message;

        public ICommand CancelCommand => throw new NotImplementedException();

        public void OnDialogClosed(DialogParameters dialogParameters)
        {
            
        }

        public void OnDialogShown(DialogParameters dialogParameters)
        {
            if (dialogParameters.TryGetValue("Message", out object? value))
                Message = value.ToString();
        }

        public void OnDialogClosing()
        {
            throw new NotImplementedException();
        }
    }
}
