using DryIoc;
using SharkSpirit.Core;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class AvaloniaDevice : ComponentBase, IDevice
    {
        private Device _device;
        private RenderTargetView _renderTargetView;
        private uint _textureId;
        public AvaloniaDevice(IContainer container)
        {
            Initialize(container);
        }

        public Device GetDevice() => _device;
        public DeviceContext GetDeviceContext() => _device.ImmediateContext;
        public uint GetTextureId() => _textureId;

        private void Initialize(IContainer services)
        {
            var angleDirectXInterop = services.Resolve<IAngleDirectXInterop>();

            var interopTexture = angleDirectXInterop.InitializeDevice();
            _device = angleDirectXInterop.GetDevice();
            _textureId = angleDirectXInterop.GetTextureId();

            angleDirectXInterop.InitializeSurface();

            _renderTargetView = new RenderTargetView(_device, interopTexture);
        }
    }
}
