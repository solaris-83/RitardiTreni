using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages;

public partial class TrainsSearchPage : BaseContentPage<TrainsSearchViewModel>
{
    public TrainsSearchViewModel Viewmodel { get; private set; } // Serve per lo XAML
    public TrainsSearchPage(TrainsSearchViewModel vm) : base(vm)	
    {
        Viewmodel = vm;
        InitializeComponent();
    }
}