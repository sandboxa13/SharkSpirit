using Avalonia.OpenGL;

namespace SharkSpirit.RenderFramework.DirectX.Avalonia
{
    public class AvaloniaInteropHelper 
    {
        public AvaloniaInteropHelper(
            IWindowingPlatformGlFeature glFeature,
            int directXVersion)
        {
            GlFeature = glFeature;
            DirectXVersion = directXVersion;
        }
        
        public IWindowingPlatformGlFeature GlFeature { get; private set; }
        public int DirectXVersion { get; private set; }
    }
}