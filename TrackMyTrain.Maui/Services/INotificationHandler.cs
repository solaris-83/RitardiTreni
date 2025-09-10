namespace TrackMyTrain.Maui.Services
{
    public interface INotificationHandler
    {
        /// <summary>
        /// Handle error in UI.
        /// </summary>
        /// <param name="ex">Exception being thrown.</param>
        void HandleError(Exception ex);

        Task<bool> HandleNotification(string title, string message, string ok, string cancel);
    }
}