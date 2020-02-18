using System;
using DryIoc;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public class DefaultDevice : IDevice
    {
        public DefaultDevice(IContainer container)
        {
            
        }

        public Device GetDevice()
        {
            throw new NotImplementedException();
        }

        public DeviceContext GetDeviceContext()
        {
            throw new NotImplementedException();
        }

        public uint GetTextureId()
        {
            throw new NotImplementedException();
        }
    }
}
