using SharkSpirit.Core;

namespace SharkSpirit.RenderFramework.DirectX.Primitives
{
    public abstract class AbstractPrimitiveBuilder
    {
        protected AbstractPrimitiveBuilder(IDevice device, IConfiguration configuration)
        {
            Configuration = configuration;
            Device = device;
        }

        protected readonly IDevice Device;
        protected readonly IConfiguration Configuration;

        protected abstract void AddVertexBufferStage(RenderObject renderObject);
        protected abstract void AddTextureStage(RenderObject renderObject);
        protected abstract void AddSamplerStage(RenderObject renderObject);
        protected abstract void AddVertexShaderStage(RenderObject renderObject);
        protected abstract void AddPixelShaderStage(RenderObject renderObject);
        protected abstract void AddIndexBufferStage(RenderObject renderObject);
        protected abstract void AddInputLayoutStage(RenderObject renderObject);
        protected abstract void AddTopologyStage(RenderObject renderObject);
        protected abstract void AddTransformConstantBufferStage(RenderObject renderObject);
    }
}
