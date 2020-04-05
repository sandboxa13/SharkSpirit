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

        public override void BindToPipeLine()
        {
            var cb = new ConstantBuffer
            {
                World = Matrix.Transpose(_renderObject.World),
                View = Matrix.Transpose(_renderObject.View),
                Projection = Matrix.Transpose(_renderObject.ViewProjection),
            };

            _vertexConstantBuffer.Update(cb, Device.GetBuffer());

            _vertexConstantBuffer.BindToPipeLine();
        }
    }
}