using CommunityToolkit.Mvvm.DependencyInjection;
using DataServiceLibrary;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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
                    .BuildServiceProvider());
        }

    }

}
