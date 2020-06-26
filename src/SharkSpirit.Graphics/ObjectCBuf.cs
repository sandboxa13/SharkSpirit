using SharpDX;

namespace SharkSpirit.Graphics
{
    public struct ObjectCBuf
    {
        public Vector3 MaterialColor;
        public float SpecularIntensity;
        public float SpecularPower;
        public Vector3 padding;
    }
}