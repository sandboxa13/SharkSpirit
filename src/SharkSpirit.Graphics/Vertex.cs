using SharpDX;

namespace SharkSpirit.Graphics
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 N;
            
        public Vertex(Vector3 position, Vector3 n)
        {
            Position = position;
            N = n;
        }
    }
}