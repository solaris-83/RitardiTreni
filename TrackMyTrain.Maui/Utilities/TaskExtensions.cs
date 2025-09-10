using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.Utilities
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task, INotificationHandler? handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                handler?.HandleError(ex);
            }
        }
    }
}
