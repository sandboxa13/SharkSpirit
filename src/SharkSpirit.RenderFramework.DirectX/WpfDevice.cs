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
        private DepthStencilView _depthView;
        private TextFormat _debugTextFormat;
        private SolidColorBrush _brush;
        private Texture2D _zBufferTexture;

        public WpfDevice(IContainer container) => _container = container;
        public Device GetDevice() => _device;
        public DeviceContext GetDeviceContext() => _immediateContext;
        public uint GetTextureId() => 0;
        public Buffer GetBuffer() => _constantBuffer;
        public Matrix GetProjection() => _projection;
        public void Flush() => _immediateContext.Flush();

        public void DrawSceneInfo(string output)
        {
            _renderTarget2D.BeginDraw();
            _renderTarget2D.DrawTextLayout(new SharpDX.Vector2(50, 50), new TextLayout(new Factory(), output, _debugTextFormat, 230, 230), _brush);
            _renderTarget2D.EndDraw();
        }

        public void Clear(TimeSpan timerTotalTime)
        {
            _immediateContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1f, 0);
            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));
        }

        public void Clear()
        {
            _immediateContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1f, 0);
            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));

            _immediateContext.Flush();
        }

        #region Initialization

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

            var cbd = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None
            };
            _constantBuffer = new Buffer(_device, cbd);
        }

        public void Reinitialize()
        {
            var windowHandle = _container.GetService<WindowHandleContainer>();

            var dxgiResourceTypeGuid = (typeof(SharpDX.DXGI.Resource)).GUID;
            Marshal.QueryInterface(windowHandle.WindowHandle, ref dxgiResourceTypeGuid, out var dxgiResource);
            var dxgiObject = new SharpDX.DXGI.Resource(dxgiResource);
            var sharedHandle = dxgiObject.SharedHandle;
            var outputResource = _device.OpenSharedResource<Texture2D>(sharedHandle);
            dxgiObject.Dispose();
            var rtDesc = new RenderTargetViewDescription
            {
                Format = Format.B8G8R8A8_UNorm,
                Dimension = RenderTargetViewDimension.Texture2D,
                Texture2D = { MipSlice = 0 }
            };

            _renderTargetView = new RenderTargetView(_device, outputResource, rtDesc);

            var t = _renderTargetView.Resource.QueryInterface<Texture2D>();
            using (var surface = t.QueryInterface<Surface>())
            {
                var properties = new RenderTargetProperties
                {
                    DpiX = 96,
                    DpiY = 96,
                    PixelFormat =
                        new PixelFormat(Format.Unknown, AlphaMode.Premultiplied),
                    Type = RenderTargetType.Default,
                    Usage = RenderTargetUsage.None
                };
                _renderTarget2D = new RenderTarget(new SharpDX.Direct2D1.Factory(), surface, properties);
            }
            using (var fontFactory = new Factory())
            {
                _debugTextFormat = new TextFormat(fontFactory, "Arial", 15f);
            }

            var zBufferTextureDescription = new Texture2DDescription
            {
                Format = Format.D32_Float,
                ArraySize = 1,
                MipLevels = 1,
                Width = (int)_configuration.Width,
                Height = (int)_configuration.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            _zBufferTexture = new Texture2D(_device, zBufferTextureDescription);
            _depthView = new DepthStencilView(_device, _zBufferTexture);

            _immediateContext.OutputMerger.SetRenderTargets(_depthView, _renderTargetView);

            var outputResourceDesc = outputResource.Description;
            if (outputResourceDesc.Width != _configuration.Width || outputResourceDesc.Height != _configuration.Height)
            {
                SetUpViewPort();
            }


            outputResource.Dispose();

            _brush = new SolidColorBrush(_renderTarget2D, new Color4(1f, 1f, 1f, 1f));
        }




        private void SetUpViewPort()
        {
            var vp = new Viewport(0, 0, (int)_configuration.Width, (int)_configuration.Height);
            _immediateContext.Rasterizer.SetViewport(vp);
            _projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, _configuration.Width / _configuration.Height, 0.01f, 100.0f);
        }

        #endregion
    }
}
