using CommunityToolkit.Maui.Views;
using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages;

public partial class CategoriesPopup : Popup<string>
{
	public CategoriesPopup(CategoriesPopupViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
		
	}
}