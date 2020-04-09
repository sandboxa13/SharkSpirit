using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharkSpirit.Modules.Core.Extensions;
using SharpDX;
using Application = System.Windows.Application;
using Container = SharkSpirit.Core.Container;

namespace SharkSpirit.Modules.Scene.Views
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView 
    {
        private bool _lastVisible;
        private Game _game;
        public SceneView()
        {
            InitializeComponent();
            InitializeSubscriptions();
        }

        private void InitializeSubscriptions()
        {
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var windowHandle = (new WindowInteropHelper(Application.Current.MainWindow ?? throw new InvalidOperationException())).Handle;

            var container = new Container();

            var graphicsConfiguration = new SharkSpirit.Core.Configuration
            {
                Height = (float)Application.Current.MainWindow.Height,
                Width = (float)Application.Current.MainWindow.Width,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders",
                MonitorHeight = (float)Screen.PrimaryScreen.Bounds.Height,
                MonitorWidth = (float)Screen.PrimaryScreen.Bounds.Width
            };
            
            container.AddService(graphicsConfiguration);

            var windowHandleContainer = new WindowHandleContainer(windowHandle);
            container.AddService(windowHandleContainer);

            if(_game != null)
                return;

            _game = new Game(container);

            var tmp = 0;

            for (var i = 0; i < 40; i++)
            {
                _game.Scene.AddEntity(new Entity(new Vector3(tmp, 0, 0), container));

                tmp += 5;
            }

            InteropImage.WindowOwner = windowHandle;
            InteropImage.OnRender = OnRender;

            InteropImage.RequestRender();
        }

        private void OnRender(IntPtr resourcePointer, bool isNewSurface)
        {
            if (isNewSurface)
            {
                _game.Reinitialize(resourcePointer);
            }

            _game.Update();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var dpiScale = 1.0; // default value for 96 dpi

            // determine DPI
            // (as of .NET 4.6.1, this returns the DPI of the primary monitor, if you have several different DPIs)
            if (PresentationSource.FromVisual(this)?.CompositionTarget is HwndTarget hwndTarget)
            {
                dpiScale = hwndTarget.TransformToDevice.M11;
            }

            var surfWidth = (int)(ActualWidth < 0 ? 0 : Math.Ceiling(ActualWidth * dpiScale));
            var surfHeight = (int)(ActualHeight < 0 ? 0 : Math.Ceiling(ActualHeight * dpiScale));

            var graphicsConfiguration = new SharkSpirit.Core.Configuration
            {
                Height = (float)surfHeight,
                Width = (float)surfWidth,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders",
                MonitorHeight = (float)Screen.PrimaryScreen.Bounds.Height,
                MonitorWidth = (float)Screen.PrimaryScreen.Bounds.Width
            };

            _game.GetContainer().RemoveService<SharkSpirit.Core.Configuration>();
            _game.GetContainer().AddService(graphicsConfiguration);

            // notify the D3D11Image and the DxRendering component of the pixel size desired for the DirectX rendering.
            InteropImage.SetPixelSize(surfWidth, surfHeight);

            var isVisible = (surfWidth != 0 && surfHeight != 0);
            if (_lastVisible == isVisible) return;

            _lastVisible = isVisible;
            if (_lastVisible)
            {
                CompositionTarget.Rendering += OnCompositionTargetRendering;
            }
            else
            {
                CompositionTarget.Rendering -= OnCompositionTargetRendering;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            InteropImage.RequestRender();
        }
    }
}
