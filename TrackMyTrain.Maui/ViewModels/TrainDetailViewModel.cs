using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui.ViewModels
{
    public partial class TrainDetailViewModel : BaseViewModel
    {
        public TrainDetailViewModel(INotificationHandler notificationHandler) : base(notificationHandler)
        {
            
        }
    }
}
