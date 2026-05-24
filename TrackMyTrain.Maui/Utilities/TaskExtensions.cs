using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Maui.Models;
using TrackMyTrain.Maui.Pages.Controls;
using TrackMyTrain.Maui.Services;
using TrackMyTrain.Maui.ViewModels;

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

    public static class TrainExtensions
    {
        public static RecentTrain? FindTrain(this IEnumerable<RecentTrainGroup> groups, string number, string stationName)
        {
            return groups.SelectMany(g => g).FirstOrDefault(t => t.Number == number && t.DepartureStationName == stationName);
        }

        public static RecentTrain? DeleteTrain(this IEnumerable<RecentTrainGroup> groups, int ID)
        {
            return groups.SelectMany(g => g).FirstOrDefault(t => t.ID == ID);
        }
    }

    //public static class UxDiversExtensions
    //{
    //    public static IServiceCollection AddTransientUxDiversPopup(this IServiceCollection services)
    //    {
    //        return services.AddTransientPopup<CustomPopup, CustomPopupViewModel>();
    //    }
    //}
}
