using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DataServiceLibrary.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using NLog;
using System.Data.SQLite;
using System.Configuration;
using DataServiceLibrary.Model;
using RitardiTreniNet7._0.Helpers;

namespace RitardiTreniNet7._0
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
                        .AddSingleton(configuration)
                        .AddSingleton<IDataService, DataService>()
                        .AddSingleton<MainViewModel>()
                        .BuildServiceProvider());
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
