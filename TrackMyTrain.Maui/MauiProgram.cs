
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SimpleFeedReader;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FA-Regular-400.otf", "FaRegular");
                    fonts.AddFont("FA-Solid-900.otf", "FaSolid");
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
            builder.Services.AddTransientPopup<CategoriesPopup, CategoriesPopupViewModel>();

            var feedReaderHttpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (cert?.Issuer != null && cert.Issuer.Contains("CN=Sectigo"))
                    {
                        return true;
                    }
                    return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                }
            })
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Then pass it to AddFeedReader
            builder.Services.AddFeedReader(() => new FeedReaderOptions()
            {
                ThrowOnError = true,
                HttpClient = feedReaderHttpClient
            });

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
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => {
                    {
                        if (cert?.Issuer != null && cert.Issuer.Contains("CN=mkcert"))
                        {
                            return true;
                        }
                        return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None;
                    }
                }
#endif
            });
            return builder.Build();
        }
    }
}
