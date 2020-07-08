using System.Runtime.InteropServices;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.Primitives.Sphere
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConstantColor
    {
        public Vector4 Color;

        public ConstantColor(Vector4 color)
        {
            Color = color;
        }
    }
}