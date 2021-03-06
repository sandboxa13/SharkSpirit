using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using SharkSpirit.Core;
using SharkSpirit.Modules.Core.AvalonDock;
using SharkSpirit.Modules.Scene.ViewModels;
using Application = System.Windows.Application;

namespace SharkSpirit.Modules.Scene.Views
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Left, IsHidden = false, Title = "Scene", Size = 1300)]
    public partial class SceneView 
    {
        private bool _lastVisible;
        private int _loadCount;
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
            _loadCount++;

            if(_loadCount > 1)
                return;

            var windowHandle = (new WindowInteropHelper(Application.Current.MainWindow ?? throw new InvalidOperationException())).Handle;

            if (DataContext is SceneViewModel dataContext)
            {
                dataContext.SetWindowHandle(windowHandle);
            }

            InteropImage.WindowOwner = windowHandle;
            InteropImage.OnRender = OnRender;

            InteropImage.RequestRender();
        }

        private void OnRender(IntPtr resourcePointer, bool isNewSurface)
        {
            if (DataContext is SceneViewModel dataContext)
            {
                dataContext.OnRender(resourcePointer, isNewSurface);
            }
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

            if (DataContext is SceneViewModel dataContext)
            {
                var bounds = BoundsRelativeTo(this);

                var graphicsConfiguration = new SharkSpirit.Core.Configuration
                {
                    Height = (float)surfHeight,
                    Width = (float)surfWidth,
                    EngineEditorType = EngineEditorType.Wpf,
                    PathToShaders = "C:\\Repositories\\BitBucket\\sharkspirit\\src\\SharkSpirit.Graphics\\Shaders",
                    MonitorHeight = (float)Screen.PrimaryScreen.Bounds.Height,
                    MonitorWidth = (float)Screen.PrimaryScreen.Bounds.Width,
                    ControlBounds = new System.Drawing.Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height)
                };

                dataContext.GetContainer().RemoveService<SharkSpirit.Core.Configuration>();
                dataContext.GetContainer().AddService(graphicsConfiguration);
            }

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

        public Rect BoundsRelativeTo(FrameworkElement element)
        {
            return
                element.TransformToVisual(this)
                    .TransformBounds(LayoutInformation.GetLayoutSlot(element));
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            CompositionTarget.Rendering -= OnCompositionTargetRendering;
        }

        private void OnCompositionTargetRendering(object sender, EventArgs e)
        {
            try
            {
                InteropImage.RequestRender();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}
