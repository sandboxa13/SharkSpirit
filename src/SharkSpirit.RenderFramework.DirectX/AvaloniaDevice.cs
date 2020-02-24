using SharkSpirit.Core;
using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class AvaloniaDevice : ComponentBase, IDevice
    {
        private readonly IContainer _container;
        private Device _device;
        private RenderTargetView _renderTargetView;
        private uint _textureId;
        public AvaloniaDevice(IContainer container)
        {
            _container = container;
        }

        public Device GetDevice() => _device;
        public DeviceContext GetDeviceContext() => _device.ImmediateContext;
        public uint GetTextureId() => _textureId;
        public Buffer GetBuffer()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            var angleDirectXInterop = _container.GetService<IAngleDirectXInterop>();

            var interopTexture = angleDirectXInterop.InitializeDevice();
            _device = angleDirectXInterop.GetDevice();
            _textureId = angleDirectXInterop.GetTextureId();

            angleDirectXInterop.InitializeSurface();

            _renderTargetView = new RenderTargetView(_device, interopTexture);
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public Matrix GetProjection()
        {
            throw new System.NotImplementedException();
        }

        public void Flush()
        {
            throw new System.NotImplementedException();
        }
    }
}
