using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using NLog;
using RitardiTreni.Common.Helpers;
using RitardiTreni.Common.Services;
using MVVMDialogsModule.Views.Interfaces;
using MVVMDialogsModule.Views.Services;
using RitardiTreniNet7.ViewModels;
using MVVMDialogsModule.Interfaces;

namespace RitardiTreniNet7
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider? Services { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                base.OnStartup(e);
                var configurationBuilder = new ConfigurationBuilder();
                IConfiguration configuration = configurationBuilder.AddJsonFile(@"Resources\appSettings.json").Build();

                Ioc.Default.ConfigureServices(new ServiceCollection()
                        //Services
                        .AddLogging(loggingBuilder =>
                        {
                            // configure Logging with NLog
                            loggingBuilder.ClearProviders();
                            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                            loggingBuilder.AddNLog(@"Resources\nlog.config");
                        })
                        .AddHttpClient("InfoMobilita", httpClient =>
                        {
                            httpClient.BaseAddress = new Uri("http://www.viaggiatreno.it/infomobilita/");
                        }).Services
                        .AddHttpClient("Resteasy", httpClient =>
                        {
                            httpClient.BaseAddress = new Uri("http://www.viaggiatreno.it/infomobilita/resteasy/viaggiatreno/");
                        }).Services
                        .AddSingleton(configuration)
                        .AddSingleton<IWindowSupport, WindowSupport>()
                        .AddTransient<IDialogService, DialogService>()
                        .AddSingleton<IDbContextService, DbContextService>()
                        .AddSingleton<IDataService, DataService>()
                        .AddTransient<MessageNotificationViewModel>()
                        .AddSingleton<MainViewModel>()
                        .BuildServiceProvider());
                DialogService.AutoRegisterDialogs<App>();
                SQLiteDbHelper.CreateDb(configuration);
            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
            base.OnExit(e);
        }
    }
}
