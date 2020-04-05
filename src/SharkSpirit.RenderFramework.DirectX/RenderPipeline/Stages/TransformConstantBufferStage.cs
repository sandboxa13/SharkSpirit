using SharkSpirit.Graphics;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TransformConstantBufferStage : StageBase
    {
        private readonly RenderObject _renderObject;
        private readonly VertexConstantBufferStage<Matrix> _vertexConstantBuffer;
        
        public TransformConstantBufferStage(IDevice device, RenderObject renderObject) : base(device)
        {
            _renderObject = renderObject;
            _vertexConstantBuffer = new VertexConstantBufferStage<Matrix>(device);
        }

        public override void BindToPipeline()
        {
            var world = _renderObject.World;

            var cb = new ConstantBuffer
            {
                World = Matrix.Transpose(world),
                View = Matrix.Transpose(Matrix.Identity),
                Projection = Matrix.Transpose(_renderObject.ViewProjection),
            };

            _vertexConstantBuffer.Update(cb, Device.GetBuffer());

            _vertexConstantBuffer.BindToPipeline();
        }
    }
}