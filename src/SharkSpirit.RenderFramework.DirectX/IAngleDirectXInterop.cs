using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX
{
    public interface IAngleDirectXInterop
    {
        uint GetTextureId();
        Texture2D InitializeDevice();
        void InitializeSurface();
        Device GetDevice();
    }
}
