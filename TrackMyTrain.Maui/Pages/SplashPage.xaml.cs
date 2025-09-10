namespace TrackMyTrain.Maui.Pages;

public partial class SplashPage : ContentPage
{
	public SplashPage()
	{
		InitializeComponent();
        SKLottieView.AnimationCompleted += SKLottieView_AnimationCompleted;

    }

    private void SKLottieView_AnimationCompleted(object? sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}