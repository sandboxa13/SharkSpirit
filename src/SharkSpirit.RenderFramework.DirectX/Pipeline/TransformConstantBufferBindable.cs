using SharkSpirit.Graphics;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class TransformConstantBufferBindable : BindableBase
    {
        private readonly RenderObject _renderObject;
        private readonly VertexConstantBufferBindable<Matrix> _vertexConstantBuffer;
        
        public TransformConstantBufferBindable(IDevice device, RenderObject renderObject) : base(device)
        {
            _renderObject = renderObject;
            _vertexConstantBuffer = new VertexConstantBufferBindable<Matrix>(device);
        }

        public override void Bind()
        {
            var cb = new ConstantBuffer
            {
                World = Matrix.Transpose(_renderObject.World),
                View = Matrix.Transpose(_renderObject.View),
                Projection = Matrix.Transpose(_renderObject.ViewProjection),
            };

            _vertexConstantBuffer.Update(cb, Device.GetBuffer());

            _vertexConstantBuffer.Bind();
        }
    }
}