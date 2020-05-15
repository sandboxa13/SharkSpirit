using System;
using SharkSpirit.Core;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages;
using SharpDX.Direct3D;

namespace SharkSpirit.RenderFramework.DirectX.Primitives.Sphere
{
    public class SpherePrimitiveBuilder : AbstractPrimitiveBuilder
    {
        public SpherePrimitiveBuilder(IDevice device, IConfiguration configuration) : base(device, configuration)
        {
        }

        protected override void AddVertexBufferStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddTextureStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddSamplerStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddVertexShaderStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddPixelShaderStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddIndexBufferStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddInputLayoutStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }

        protected override void AddTopologyStage(RenderObject renderObject)
        {
            renderObject.AddStage(new TopologyStage(Device, PrimitiveTopology.TriangleList));
        }

        protected override void AddTransformConstantBufferStage(RenderObject renderObject)
        {
            throw new NotImplementedException();
        }
    }
}
