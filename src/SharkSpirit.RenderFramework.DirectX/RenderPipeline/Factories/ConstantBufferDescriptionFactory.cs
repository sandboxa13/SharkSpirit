using SharkSpirit.Graphics;
using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories
{
    public static class ConstantBufferDescriptionFactory
    {
        public static BufferDescription CreateConstantBufferDescription()
        {
            var cbd = new BufferDescription
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<ConstantBuffer>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
            };

            return cbd;
        }
    }
}