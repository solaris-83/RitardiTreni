using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages;

public partial class TrainsSearchPage : BaseContentPage<TrainsSearchViewModel>
{
    private readonly TrainsSearchViewModel _viewModel;
    public TrainsSearchPage(TrainsSearchViewModel vm) : base(vm)	
    {
        InitializeComponent();
        _viewModel = vm;
        _viewModel.SearchTrainCompleted += _viewModel_SearchTrainCompleted;
    }

    private void _viewModel_SearchTrainCompleted(object? sender, EventArgs e)
    {
        collectionView.ScrollTo(0, -1, ScrollToPosition.Start, true);
    }

    protected override void OnDisappearing()
    {
        _viewModel.SearchTrainCompleted -= _viewModel_SearchTrainCompleted;
    }
}