using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages.Views;

public partial class TrainsSelectionPopup : ContentView
{
	public TrainsSelectionPopup(TrainDetailViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}