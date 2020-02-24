using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline.Factories
{
    public static class VertexBufferDescriptionFactory
    {
        public static BufferDescription CreateVertexBufferDescription<T>(int count) where T : struct
        {
            var vbd = new BufferDescription
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<T>() * count,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
            };

            return vbd;
        }
    }
}