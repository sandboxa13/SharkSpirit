using SharkSpirit.Core;
using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class WpfDevice : IDevice
    {
        private readonly IContainer _container;

        public WpfDevice(IContainer container)
        {
            _container = container;
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

        public Buffer GetBuffer()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            
        }

        public void Clear()
        {

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
