
using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui.Pages
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel
    {
       
        private readonly TViewModel _viewModel;
        protected BaseContentPage(TViewModel viewModel)
        {
            base.BackgroundColor = Color.FromArgb("#e5e5e5");
            base.BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected new TViewModel BindingContext => (TViewModel)base.BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Appearing();
        }
    }
}
