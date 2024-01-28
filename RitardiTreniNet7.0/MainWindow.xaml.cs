using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;

namespace RitardiTreniNet7._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
        }
    }
}