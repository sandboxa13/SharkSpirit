using System;
using System.Runtime.InteropServices;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class DefaultDevice : IDevice
    {
        private readonly IContainer _container;
        private DeviceContext _immediateContext;
        private Device _device;
        private Buffer _constantBuffer;
        private RenderTargetView _renderTargetView;
        private Matrix _projection;
        private Matrix _view;
        private IConfiguration _configuration;

        public DefaultDevice(IContainer container)
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
        
        public void Initialize()
        {
            var windowHandleContainer = _container.GetService<WindowHandleContainer>();
            _configuration = _container.GetService<IConfiguration>();
            
            var createDeviceFlag = DeviceCreationFlags.BgraSupport;
            var driverTypes = new[] {DriverType.Hardware, DriverType.Warp, DriverType.Reference};
            var featureLevels = new[] {FeatureLevel.Level_11_0};
            foreach (var dt in driverTypes)
            {
                try
                {
                    _device = new Device(dt, createDeviceFlag, featureLevels);
                }
                catch (Exception)
                {
                    continue;
                }

                break;
            }

            _immediateContext = _device.ImmediateContext;
            
            var rtDesc = new RenderTargetViewDescription
            {
                Format = Format.B8G8R8A8_UNorm,
                Dimension = RenderTargetViewDimension.Texture2D,
                Texture2D = {MipSlice = 0}
            };
            
            var dxgiResourceTypeGuid = (typeof(SharpDX.DXGI.Resource)).GUID;
            Marshal.QueryInterface(windowHandleContainer.WindowHandle, ref dxgiResourceTypeGuid, out var dxgiResource);
            var dxgiObject = new SharpDX.DXGI.Resource(dxgiResource);
            var sharedHandle = dxgiObject.SharedHandle;
            var outputResource = _device.OpenSharedResource<Texture2D>(sharedHandle);
            dxgiObject.Dispose();
            
            _renderTargetView = new RenderTargetView(_device, outputResource, rtDesc);
            
            SetUpViewPort();
            
            var cbd = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None
            };
            _constantBuffer = new Buffer(_device, cbd);
            
            var eye = new Vector3(0, 1, -5);
            var at = new Vector3(0, 1, 0);
            var up = new Vector3(0, 1, 0);
            _view = Matrix.LookAtLH(eye, at, up);
        }

        public void Clear()
        {
            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));
            _immediateContext.OutputMerger.SetRenderTargets(null, _renderTargetView);
        }

        public void Flush()
        {
            _immediateContext?.Flush();
        }

        public void SetUpViewPort()
        {
            var vp = new Viewport(0, 0, (int) _configuration.Width, (int) _configuration.Height, 0f, 1f);
            _immediateContext.Rasterizer.SetViewport(vp);
            _projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, _configuration.Width / _configuration.Height, 0.01f, 100f);
        }
    }
}
