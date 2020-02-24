using SharkSpirit.RenderFramework.DirectX.Pipeline.Factories;
using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class VertexBufferBindable<T> : ConstantBufferBindable<T> where T : unmanaged
    {
        private readonly Buffer _vertexBuffer;

        public VertexBufferBindable(IDevice device, T[] vertices) : base(device)
        {
            var vbd = VertexBufferDescriptionFactory.CreateVertexBufferDescription<T>(vertices.Length);

            _vertexBuffer = Buffer.Create(device.GetDevice(), vertices, vbd);
        }

        public override void Bind()
        {
            Device.GetDeviceContext().InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<T>(), 0));
        }
    }
}