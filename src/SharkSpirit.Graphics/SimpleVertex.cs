using System.Runtime.InteropServices;
using SharpDX;

namespace SharkSpirit.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SimpleVertex
    {
        public Vector4 Position;
        public Vector2 TextureUV;
        public SimpleVertex(Vector4 position, Vector2 textureUv)
        {
            Position = position;
            TextureUV = textureUv;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleIndex
    {
        public TriangleIndex(uint a, uint b, uint c)
        {
            A = a;
            B = b;
            C = c;
        }

        public uint A;
        public uint B;
        public uint C;
    }
}