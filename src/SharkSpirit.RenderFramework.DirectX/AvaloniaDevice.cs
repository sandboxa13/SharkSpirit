using System;
using SharkSpirit.Core;
using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.Avalonia;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class AvaloniaDevice : ComponentBase, IDevice
    {
        private Device _device;
        private DeviceContext _immediateContext;
        private Texture2D _texture;
        private RenderTargetView _renderTargetView;
        private readonly AvaloniaInterop _avaloniaInterop;
        private readonly IContainer _container;
        private IConfiguration _configuration;
        private Matrix _projection;
        private Buffer _constantBuffer;
        private Matrix _world;
        private Matrix _view;

        public AvaloniaDevice(
            IContainer container,
            AvaloniaInteropHelper avaloniaInteropHelper) : base(container, "")
        {
            _container = container;
            _avaloniaInterop = new AvaloniaInterop(container, avaloniaInteropHelper.GlFeature, avaloniaInteropHelper.DirectXVersion);
        }

        public Device GetDevice() => _device;
        public DeviceContext GetDeviceContext() => _device.ImmediateContext;
        public uint GetTextureId() => _avaloniaInterop.GetTextureId();
        public Buffer GetBuffer() => _constantBuffer;

        public void Initialize()
        {
            _configuration = _container.GetService<IConfiguration>();

            _avaloniaInterop.Initialize();

            _device = new Device(_avaloniaInterop.GetDevicePtr());
            _immediateContext = _device.ImmediateContext;

            _texture = new Texture2D(_device, new Texture2DDescription
            {
                Width = (int) _configuration.Width,
                Height = (int) _configuration.Height,
                ArraySize = 1,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Usage = ResourceUsage.Default,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                MipLevels = 1,
                OptionFlags = ResourceOptionFlags.Shared,
                SampleDescription = new SampleDescription(1, 0)
            });

            _renderTargetView = new RenderTargetView(_device, _texture);

            _avaloniaInterop.InitializeSurface(_texture.NativePointer);

            SetUpViewport();

            _immediateContext.OutputMerger.SetRenderTargets(null, _renderTargetView);

            var cbd = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None
            };
            _constantBuffer = new Buffer(_device, cbd);

            _world = Matrix.Identity;

            var eye = new Vector3(0, 1, -5);
            var at = new Vector3(0, 1, 0);
            var up = new Vector3(0, 1, 0);
            _view = Matrix.LookAtLH(eye, at, up);
        }

        private void SetUpViewport()
        {
            var vp = new Viewport(0, 0, (int) _configuration.Width,
                (int) _configuration.Height, 0f, 1f);
            _immediateContext.Rasterizer.SetViewport(vp);
            _projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour,
                (float) _configuration.Width /
                (float) _configuration.Height, 0.01f, 100f);
        }

        public void Clear()
        {
            _immediateContext
                .ClearRenderTargetView(_renderTargetView, new Color4(0.07f, 0.0f, 0.12f, 1.0f));
        }

        public Matrix GetProjection() => _projection;

        public void Flush()
        {
            throw new System.NotImplementedException();
        }

        public void Clear(TimeSpan timerTotalTime)
        {
            throw new NotImplementedException();
        }

        public void Reinitialize()
        {
            throw new NotImplementedException();
        }

        public void DrawSceneInfo(string output)
        {
            throw new NotImplementedException();
        }

        public Matrix GetView()
        {
            return Matrix.Identity;
        }
    }
}