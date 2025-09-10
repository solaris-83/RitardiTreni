using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages;

public partial class StationsSearchPage : BaseContentPage<StationsSearchViewModel>
{
	public StationsSearchPage(StationsSearchViewModel vm) : base(vm)
	{
		InitializeComponent();
	}
}