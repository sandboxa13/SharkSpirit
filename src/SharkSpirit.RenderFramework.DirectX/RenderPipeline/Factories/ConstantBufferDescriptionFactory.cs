using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories
{
    public static class ConstantBufferDescriptionFactory
    {
        public static BufferDescription CreateConstantBufferDescription<T>() where T : unmanaged
        {
            var cbd = new BufferDescription
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = Utilities.SizeOf<T>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
            };

            return cbd;
        }
    }
}