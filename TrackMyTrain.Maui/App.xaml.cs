using Microsoft.Extensions.Logging;
#if WINDOWS
    using Microsoft.UI.Windowing;
#endif
using TrackMyTrain.Data.Implementations;
using TrackMyTrain.Data.Interfaces;
using TrackMyTrain.Maui.Pages;
using TrackMyTrain.Maui.Services;

namespace TrackMyTrain.Maui
{
    public partial class App : Application
    {
        const int WindowWidth = 920;
        const int WindowHeight = 900;

        private readonly ILogger<App> _logger;
        private readonly IDataService _dataService;
        public List<StationLocation> CachedStations { get; private set; } = new List<StationLocation>();
        public App(IDataService dataService, ILogger<App> logger)
        {
            _logger = logger;
            _dataService = dataService;
            _ = Task.Run(async () =>
            {
                try
                {
                    await _dataService.LoadCachedStationsAsync();
                }
                catch (Exception ex)
                {
                   _logger.LogError(ex, "Failed to load stations.");
                }
            });
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
#if WINDOWS

                var mauiWindow = handler.VirtualView;
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
                appWindow.Resize(new Windows.Graphics.SizeInt32(WindowWidth, WindowHeight));
                if (appWindow.Presenter is OverlappedPresenter presenter)
                {
                    presenter.IsResizable = false;
                    presenter.IsMaximizable = false;
                    presenter.IsMinimizable = false;
                }
#endif
            });

            return new Window(new AppShell());
        }
    }
}