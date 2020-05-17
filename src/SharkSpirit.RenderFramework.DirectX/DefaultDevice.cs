using System;
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
        private IConfiguration _configuration;
        private SwapChain _swapChain;
        private Texture2D _depthBuffer;
        private DepthStencilView _depthView;

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

        public Buffer GetPixelBuffer()
        {
            throw new NotImplementedException();
        }

        public Matrix GetProjection()
        {
            return _projection;
        }
        
        public void Initialize()
        {
            _configuration = _container.GetService<Core.Configuration>();
            var windowHandleContainer = _container.GetService<WindowHandleContainer>();

            var backBufferDesc = new ModeDescription((int)_configuration.Width, (int)_configuration.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm);

            var swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = windowHandleContainer.WindowHandle,
                IsWindowed = true
            };

            using (var factory = new Factory1())
            {
                var adapter = factory.Adapters1[0];
                var featureLevels = new[] {
                    FeatureLevel.Level_11_0,
                    FeatureLevel.Level_10_1,
                    FeatureLevel.Level_10_0,
                    FeatureLevel.Level_9_3,
                };
                Device.CreateWithSwapChain(adapter, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport, featureLevels, swapChainDesc, out _device, out _swapChain);
                adapter.Dispose();
            }

            _immediateContext = _device.ImmediateContext;

            using (var backBuffer = _swapChain.GetBackBuffer<Texture2D>(0))
            {
                _renderTargetView = new RenderTargetView(_device, backBuffer);
            }


            var cbd = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None
            };

            _constantBuffer = new Buffer(_device, cbd);


            _depthBuffer = new Texture2D(_device, new Texture2DDescription()
            {
                Format = Format.D32_Float_S8X24_UInt,
                ArraySize = 1,
                MipLevels = 1,
                Width = (int)_configuration.Width,
                Height = (int)_configuration.Height,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            _depthView = new DepthStencilView(_device, _depthBuffer);

            SetUpViewPort();
        }

        public void Flush()
        {
            _swapChain.Present(1, PresentFlags.None);
        }

        public void Clear(TimeSpan timerTotalTime)
        {
            var c = (float)(Math.Sin(timerTotalTime.TotalSeconds) / 2.0f + 0.5f);

            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(c, c, 1, 1.0f));
        }

        public void Reinitialize()
        {
            throw new NotImplementedException();
        }

        public void DrawSceneInfo(string output)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            _immediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));
        }

        public void SetUpViewPort()
        {
            var vp = new Viewport(0, 0, (int) _configuration.Width, (int) _configuration.Height);
            _immediateContext.Rasterizer.SetViewport(vp);
            _projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, _configuration.Width / _configuration.Height, 0.01f, 100.0f);
        }
    }
}
