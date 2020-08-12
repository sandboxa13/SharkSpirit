using SharpDX;

namespace SharkSpirit.Graphics
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 N;
        public Vector2 Texture;
            
        public Vertex(Vector3 position, Vector3 n, Vector2 texture)
        {
            Position = position;
            N = n;
            Texture = texture;
        }
    }
}