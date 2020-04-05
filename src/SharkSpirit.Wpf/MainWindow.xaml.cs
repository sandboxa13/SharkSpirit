using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using SharkSpirit.Core;
using SharkSpirit.Engine;
using SharpDX;
using Configuration = SharkSpirit.Core.Configuration;

namespace SharkSpirit.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _lastVisible;
        private Game _game;
        public MainWindow()
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

            var container = new Core.Container();

            var graphicsConfiguration = new Configuration
            {
                Height = 1080,
                Width = 1920,
                EngineEditorType = EngineEditorType.Wpf,
                PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders"
            };

            container.AddService(graphicsConfiguration);

            var windowHandleContainer = new WindowHandleContainer(windowHandle);
            container.AddService(windowHandleContainer);

            _game = new Game(container);

            _game.Scene.AddEntity(new Entity(new Vector3(0, 0, 0)));
            _game.Scene.AddEntity(new Entity(new Vector3(0, 0, 5)));

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