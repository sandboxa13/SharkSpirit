using System;
using System.Runtime.InteropServices;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Buffer = SharpDX.Direct3D11.Buffer;
using Configuration = SharkSpirit.Core.Configuration;
using Device = SharpDX.Direct3D11.Device;
using DeviceContext = SharpDX.Direct3D11.DeviceContext;
using Factory = SharpDX.DirectWrite.Factory;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class WpfDevice : IDevice
    {
        private readonly IContainer _container;
        private DeviceContext _immediateContext;
        private Device _device;
        private Buffer _constantBuffer;
        private RenderTargetView _renderTargetView;
        private RenderTarget _renderTarget2D;
        private Matrix _projection;
        private IConfiguration _configuration;
        public WpfDevice(IContainer container)
        {
            _container = container;
        }

        public Device GetDevice()
        {
            return _device;
        }

        public DeviceContext GetDeviceContext()
        {
            return _immediateContext;
        }

        public uint GetTextureId()
        {
            return 0;
        }

        public Buffer GetBuffer()
        {
            return _constantBuffer;
        }

        public Matrix GetProjection()
        {
            return _projection;
        }

        public void Flush()
        {
            _immediateContext.Flush();
        }

        public void Initialize()
        {
            _configuration = _container.GetService<Configuration>();

            var createDeviceFlag = DeviceCreationFlags.BgraSupport;
            var driverTypes = new[] { DriverType.Hardware, DriverType.Warp, DriverType.Reference };
            var featureLevels = new[] { FeatureLevel.Level_11_0, FeatureLevel.Level_10_1, FeatureLevel.Level_10_0 };
            foreach (var dt in driverTypes)
            {
                try
                {
                    _device = new Device(dt, createDeviceFlag, featureLevels);
                }
                catch (Exception e)
                {
                    continue;
                }
                break;
            }

            _immediateContext = _device.ImmediateContext;

            Console.WriteLine("Setting render targets");
            _immediateContext.OutputMerger.SetRenderTargets(null, _renderTargetView);

            Console.WriteLine("Set up viewport");
            SetUpViewPort();

            var cbd = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None
            };
            _constantBuffer = new Buffer(_device, cbd);
        }

        public void Clear(TimeSpan timerTotalTime)
        {
            //var c = (float)(Math.Sin(timerTotalTime.TotalSeconds) / 2.0f + 0.5f);
            //var c1 = (float)(Math.Sin(timerTotalTime.TotalSeconds) / 2.0f + 1.5f);

            //_immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(c1, c, 1, 1.0f));

            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));

            var brush = new SharpDX.Direct2D1.SolidColorBrush(_renderTarget2D, new SharpDX.Color4(1f, 1f, 1f, 1f));
            _renderTarget2D.BeginDraw();
            _renderTarget2D.DrawTextLayout(new SharpDX.Vector2(50, 50),new TextLayout(new Factory(), "dfgdfsg", DebugTextFormat, 30, 30) , brush);
            _renderTarget2D.EndDraw();
        }

        public void Reinitialize()
        {
            var windowHandle = _container.GetService<WindowHandleContainer>();

            var dxgiResourceTypeGuid = (typeof(SharpDX.DXGI.Resource)).GUID;
            Marshal.QueryInterface(windowHandle.WindowHandle, ref dxgiResourceTypeGuid, out var dxgiResource);
            var dxgiObject = new SharpDX.DXGI.Resource(dxgiResource);
            var sharedHandle = dxgiObject.SharedHandle;
            var outputResource = _device.OpenSharedResource<SharpDX.Direct3D11.Texture2D>(sharedHandle);
            dxgiObject.Dispose();
            var rtDesc = new RenderTargetViewDescription
            {
                Format = Format.B8G8R8A8_UNorm,
                Dimension = RenderTargetViewDimension.Texture2D,
                Texture2D = { MipSlice = 0 }
            };

            _renderTargetView = new RenderTargetView(_device, outputResource, rtDesc);

            Texture2D t = _renderTargetView.Resource.QueryInterface<Texture2D>();
            using (var surface = t.QueryInterface<SharpDX.DXGI.Surface>())
            {
                var properties = new RenderTargetProperties();
                properties.DpiX = 96;
                properties.DpiY = 96;
                properties.PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.Unknown, AlphaMode.Premultiplied);
                properties.Type = RenderTargetType.Default;
                properties.Usage = RenderTargetUsage.None;
                _renderTarget2D = new RenderTarget(new SharpDX.Direct2D1.Factory(), surface, properties);
            }
            using (var fontFactory = new SharpDX.DirectWrite.Factory())
            {
                DebugTextFormat = new TextFormat(fontFactory, "Arial", 12f);
            }

            var outputResourceDesc = outputResource.Description;
            if (outputResourceDesc.Width != _configuration.Width || outputResourceDesc.Height != _configuration.Height)
            {
                SetUpViewPort();
            }
            _immediateContext.OutputMerger.SetRenderTargets(null, _renderTargetView);
            outputResource.Dispose();
        }

        public TextFormat DebugTextFormat { get; set; }

        public void Clear()
        {
            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));
        }

        public void SetUpViewPort()
        {
            var vp = new Viewport(0, 0, (int)_configuration.Width, (int)_configuration.Height);
            _immediateContext.Rasterizer.SetViewport(vp);
            _projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, _configuration.Width / _configuration.Height, 0.01f, 100.0f);
        }
    }
}
