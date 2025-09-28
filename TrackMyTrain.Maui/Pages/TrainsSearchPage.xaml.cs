using TrackMyTrain.Maui.ViewModels;
using System.Linq;

namespace TrackMyTrain.Maui.Pages;

public partial class TrainsSearchPage : BaseContentPage<TrainsSearchViewModel>
{
    private readonly TrainsSearchViewModel _viewModel;
    public TrainsSearchPage(TrainsSearchViewModel vm) : base(vm)	
    {
        InitializeComponent();
        _viewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.SearchTrainCompleted += _viewModel_SearchTrainCompleted;
    }

    private void _viewModel_SearchTrainCompleted(object? sender, int e)
    {
        if (e > 0)
           collectionView.ScrollTo(0, -1, ScrollToPosition.Start, true);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.SearchTrainCompleted -= _viewModel_SearchTrainCompleted;
        foreach (var recentTrain in _viewModel.RecentTrains)
        {
            foreach (var train in recentTrain)
            {
                train.PropertyChanged -= _viewModel.Train_PropertyChanged;
            }
        }
    }
}