
using TrackMyTrain.Maui.Utilities;

namespace TrackMyTrain.Maui.Services
{
    public class ModalNotificationHandler : INotificationHandler
    {
        SemaphoreSlim _semaphore = new(1, 1);

        /// <summary>
        /// Handle error in UI.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public void HandleError(Exception ex)
        {
            DisplayAlert(ex).FireAndForgetSafeAsync();
        }

        public async Task<bool> HandleNotification(string title, string message, string ok, string cancel)
        {
            return await DisplayAlert(title, message, ok, cancel);
        }

        async Task DisplayAlert(Exception ex)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (Shell.Current is Shell shell)
                    await shell.DisplayAlert("Error", ex.Message, "OK");
                else if (Shell.Current is null)
                {
                    var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                    if (currentPage != null)
                    {
                        await currentPage.DisplayAlert("Error", ex.Message, "OK");
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        async Task<bool> DisplayAlert(string title, string message, string ok, string cancel)
        {
            bool result = false;
            try
            {
                await _semaphore.WaitAsync();
                if (Shell.Current is Shell shell)
                    result = await shell.DisplayAlert(title, message, ok, cancel);
                else if (Shell.Current is null)
                {
                    var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                    if (currentPage != null)
                    {
                        result = await currentPage.DisplayAlert(title, message, ok, cancel);
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
            return result;
        }
    }
}
