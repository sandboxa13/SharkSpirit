using SharpDX;

namespace SharkSpirit.Graphics
{
    public struct LightCBuf
    {
        public Vector3 LightPos;
        public float DiffuseIntensity;

        public Vector3 Ambient;
        public float AttConst;

        public Vector3 DiffuseColor;
        public float AttLin;
        //public float AttQuad;
    }
}