using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages.Controls;

public partial class CustomPopup : ContentView
{
	public CustomPopup(CustomPopupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}