
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SimpleFeedReader;
using SkiaSharp.Views.Maui.Controls.Hosting;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Data.Services;
using TrackMyTrain.Maui.Pages;
using TrackMyTrain.Maui.Pages.Controls;
using TrackMyTrain.Maui.Pages.Views;
using TrackMyTrain.Maui.Services;
using TrackMyTrain.Maui.ViewModels;

namespace TrackMyTrain.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureMauiHandlers(handlers =>
                {
                   
                })
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<LocalDbService>();
            builder.Services.AddSingleton<INotificationHandler, ModalNotificationHandler>();
            builder.Services.AddSingleton<IRssReaderService, RssReaderService>();
            builder.Services.AddSingleton<IDataService, DataService>();
            builder.Services.AddSingleton<IApiClientService, ApiClientService>();
            builder.Services.AddSingleton<IHttpDataService, HttpDataService>();
            builder.Services.AddFeedReader(() => new FeedReaderOptions() { ThrowOnError = true });
            builder.Services.AddTransient<RssFeedPage, RssFeedViewModel>();
            builder.Services.AddTransient<StationsSearchPage, StationsSearchViewModel>();
            builder.Services.AddTransient<TrainsSearchPage, TrainsSearchViewModel>();
            builder.Services.AddTransient<TrainDetailPage, TrainDetailViewModel>();
            builder.Services.AddTransientPopup<CustomPopup, CustomPopupViewModel>();
            builder.Services.AddTransientPopup<TrainsSelectionPopup, TrainsSelectionViewModel>();

            // Here register all routes being outside the Shell
            Routing.RegisterRoute("traindetails", typeof(TrainDetailPage));

            builder.Services.AddHttpClient("api", config => { config.BaseAddress = new Uri("http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/"); }).ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler

            // Ensure these namespaces are included at the top of the file
            {
#if DEBUG
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
#endif
            });
            return builder.Build();
        }
    }
}
