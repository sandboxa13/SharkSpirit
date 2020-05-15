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
}