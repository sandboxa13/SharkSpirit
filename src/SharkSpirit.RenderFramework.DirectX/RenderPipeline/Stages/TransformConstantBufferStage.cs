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
            var world = Matrix.Identity;

            var eye = new Vector3(0, 1, -5);
            var at = new Vector3(0, 1, 0);
            var up = new Vector3(0, 1, 0);
            var view = Matrix.LookAtLH(eye, at, up);

            var cb = new ConstantBuffer
            {
                World = Matrix.Transpose(world),
                View = Matrix.Transpose(view),
                Projection = Matrix.Transpose(_renderObject.ViewProjection),
            };

            _vertexConstantBuffer.Update(cb, Device.GetBuffer());

            _vertexConstantBuffer.BindToPipeline();
        }
    }
}