using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages
{
    public partial class TrainDetailPage : BaseContentPage<BaseViewModel>
    {
        public TrainDetailPage(TrainDetailViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}
