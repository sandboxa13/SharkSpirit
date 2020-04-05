using Avalonia;
using Avalonia.Controls;
using Avalonia.OpenGL;
using SharkSpirit.Core;
using SharkSpirit.Core.Utilities;
using SharkSpirit.RenderFramework.DirectX.Avalonia;

namespace SharkSpirit.Avalonia
{
    public class Bootstrapper
    {
        public void Run()
        {
            // var container = new Container();
            //
            // var graphicsConfiguration = new Configuration
            // {
            //     Height = 1080,
            //     Width = 1920,
            // };
            //
            // container.AddService(graphicsConfiguration);
            //
            // var glFeature = AvaloniaLocator.Current.GetService<IWindowingPlatformGlFeature>();
            // var avaloniaInteropHelper = new AvaloniaInteropHelper(glFeature, SharkSpiritEglConsts.EGL_D3D11_DEVICE_ANGLE);
            //
            // container.AddService(avaloniaInteropHelper);
            //
            //
            // var mainWindowVm = new MainWindowViewModel(container);
            // mainWindow.DataContext = mainWindowVm;
            // mainWindow.Show();
            //
            // Application.Current.Run(mainWindow);  
        }
    }
}