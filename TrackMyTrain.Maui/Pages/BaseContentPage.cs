
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
    {
       
        private readonly TViewModel _viewModel;
        protected BaseContentPage(TViewModel viewModel)
        {
            base.BackgroundColor = Color.FromArgb("#e5e5e5");
            this.Behaviors.Add(new StatusBarBehavior
            {
                StatusBarColor = Colors.Green,
                StatusBarStyle = StatusBarStyle.LightContent
            });
            base.BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected new TViewModel BindingContext => (TViewModel)base.BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Appearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.Disappearing();
        }
    }
}
