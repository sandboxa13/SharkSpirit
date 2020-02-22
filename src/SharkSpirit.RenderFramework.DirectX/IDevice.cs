using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public interface IDevice
    {
        Device GetDevice();
        DeviceContext GetDeviceContext();
        uint GetTextureId();
        Buffer GetBuffer();
    }
}