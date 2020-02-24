using System.Runtime.InteropServices;
using SharpDX;

namespace SharkSpirit.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SimpleVertex
    {
        public Vector3 Position;
        public Vector4 Color;

        public SimpleVertex(Vector3 position, Vector4 color)
        {
            Position = position;
            Color = color;
        }
    }
}