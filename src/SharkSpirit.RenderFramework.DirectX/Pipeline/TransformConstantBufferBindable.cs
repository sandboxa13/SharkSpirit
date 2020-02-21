using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class TransformConstantBufferBindable : BindableBase
    {
        private readonly VertexConstantBufferBindable<Matrix> _vertexConstantBuffer;
        
        public TransformConstantBufferBindable(IDevice device, RenderObject renderObject) : base(device)
        {
        }

        public override void Bind()
        {
            
        }
    }
}