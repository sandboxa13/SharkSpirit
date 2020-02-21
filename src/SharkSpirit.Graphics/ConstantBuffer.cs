using System.Runtime.InteropServices;
using SharpDX;

namespace SharkSpirit.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConstantBuffer
    {
        public Matrix World;
        public Matrix View;
        public Matrix Projection;
    }
}