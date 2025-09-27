using CommunityToolkit.Maui.Views;
using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages.Controls;

public partial class CustomPopup : Popup
{
	public CustomPopup(CustomPopupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}