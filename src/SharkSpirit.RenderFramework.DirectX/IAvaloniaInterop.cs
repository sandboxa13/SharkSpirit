using System;

namespace SharkSpirit.RenderFramework.DirectX
{
    public interface IAvaloniaInterop
    {
        uint GetTextureId();
        void InitializeSurface(IntPtr texturePtr);
        IntPtr GetDevicePtr();
    }
}
