using SharkSpirit.Graphics;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TransformConstantBufferStage<T> : StageBase where T : unmanaged
    {
        private readonly RenderObject _renderObject;
        private readonly VertexConstantBufferStage<T> _vertexConstantBuffer;
        
        public TransformConstantBufferStage(IDevice device, RenderObject renderObject, int slot = 0) : base(device)
        {
            _renderObject = renderObject;
            _vertexConstantBuffer = new VertexConstantBufferStage<T>(device, slot);
        }

        public override void BindToPipeline()
        {
            var modelView = _renderObject.World * _renderObject.View;
            
            var cb = new TransformBuffer
            {
                model = Matrix.Transpose(modelView),
                modelViewProj = Matrix.Transpose(modelView * _renderObject.ViewProjection)
            };

            _vertexConstantBuffer.Update(cb);

            _vertexConstantBuffer.BindToPipeline();
        }
    }
}