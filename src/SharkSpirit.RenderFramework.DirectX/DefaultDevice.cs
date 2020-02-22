using System;
using DryIoc;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

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

        public Buffer GetBuffer()
        {
            throw new NotImplementedException();
        }
    }
}
