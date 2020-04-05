using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories
{
    public static class IndexBufferDescriptionFactory
    {
        public static BufferDescription CreateIndexBufferDescription(int indicesLength)
        {
            var description = new BufferDescription
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ushort>() * indicesLength,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            return description;
        }
    }
}