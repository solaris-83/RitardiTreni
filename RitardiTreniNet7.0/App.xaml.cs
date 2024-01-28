using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DataServiceLibrary.Services;

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
            base.OnStartup(e);
            Ioc.Default.ConfigureServices(new ServiceCollection()
                    //Services
                    .AddSingleton<IDataService, DataService>()
                    .AddSingleton<MainViewModel>()
                    .BuildServiceProvider());
        }
    }
}
