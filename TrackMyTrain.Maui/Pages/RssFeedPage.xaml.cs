using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages;

public partial class RssFeedPage : BaseContentPage<RssFeedViewModel>
{
	public RssFeedPage(RssFeedViewModel vm): base(vm)
    {
	  	 InitializeComponent();
	}
}