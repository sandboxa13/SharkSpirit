using DryIoc;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class WpfDevice : IDevice
    {
        public WpfDevice(IContainer container)
        {
            
        }

        public Device GetDevice()
        {
            throw new System.NotImplementedException();
        }

        public DeviceContext GetDeviceContext()
        {
            throw new System.NotImplementedException();
        }

        public uint GetTextureId()
        {
            throw new System.NotImplementedException();
        }
    }
}
