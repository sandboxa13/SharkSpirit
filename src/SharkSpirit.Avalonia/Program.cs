using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging.Serilog;
using Avalonia.OpenGL;
using SharkSpirit.Core;
using SharkSpirit.Core.Utilities;
using SharkSpirit.RenderFramework.DirectX.Avalonia;

namespace SharkSpirit.Avalonia
{
    class Program
    {
        private static AppBuilder AppBuilderInstance;
        
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args)
        {
            AppBuilderInstance = BuildAvaloniaApp();
            
            var container = new Container();
            
            var graphicsConfiguration = new Configuration
            {
                Height = 1080,
                Width = 1920,
                EngineEditorType = EngineEditorType.Avalonia
            };
            
            container.AddService(graphicsConfiguration);
            
            var glFeature = AvaloniaLocator.Current.GetService<IWindowingPlatformGlFeature>();
            var avaloniaInteropHelper = new AvaloniaInteropHelper(glFeature, SharkSpiritEglConsts.EGL_D3D11_DEVICE_ANGLE);
            
            container.AddService(avaloniaInteropHelper);
            
            
            var mainWindowVm = new MainWindowViewModel(container);
            var mainWindow = new MainWindow {DataContext = mainWindowVm};
            mainWindow.Show();
            Application.Current.Run(mainWindow);  
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Win32PlatformOptions {AllowEglInitialization = true})
                .With(new X11PlatformOptions {UseGpu = true, UseEGL = true})
                .With(new AvaloniaNativePlatformOptions {UseGpu = true})
                .With(new AngleOptions {AllowedPlatformApis = new List<AngleOptions.PlatformApi> {AngleOptions.PlatformApi.DirectX11}})
                .LogToDebug()
                .SetupWithoutStarting();
    }
}
