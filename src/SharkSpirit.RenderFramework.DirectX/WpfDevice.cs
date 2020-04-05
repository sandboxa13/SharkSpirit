using System;
using System.Runtime.InteropServices;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Configuration = SharkSpirit.Core.Configuration;
using Device = SharpDX.Direct3D11.Device;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class WpfDevice : IDevice
    {
        private readonly IContainer _container;
        private DeviceContext _immediateContext;
        private Device _device;
        private Buffer _constantBuffer;
        private RenderTargetView _renderTargetView;
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

            var outputResourceDesc = outputResource.Description;
            if (outputResourceDesc.Width != _configuration.Width || outputResourceDesc.Height != _configuration.Height)
            {
                SetUpViewPort();
            }
            _immediateContext.OutputMerger.SetRenderTargets(null, _renderTargetView);
            outputResource.Dispose();
        }

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
