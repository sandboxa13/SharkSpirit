using System.Runtime.InteropServices;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.Primitives.Sphere
{
    public class Sphere : RenderObject
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public Vector3 Position;
            
            public Vertex(Vector3 position, Vector3 n)
            {
                Position = position;
            }
        }
        public Sphere(IDevice device) : base(device, MeshType.Sphere)
        {
        }
    }
}
