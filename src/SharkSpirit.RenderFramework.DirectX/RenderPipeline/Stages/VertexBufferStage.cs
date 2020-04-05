using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories;
using SharpDX;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class VertexBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly Buffer _vertexBuffer;

        public VertexBufferStage(IDevice device, T[] vertices) : base(device)
        {
            var vbd = VertexBufferDescriptionFactory.CreateVertexBufferDescription<T>(vertices.Length);

            _vertexBuffer = Buffer.Create(device.GetDevice(), vertices, vbd);
        }

        public override void BindToPipeLine()
        {
            Device.GetDeviceContext().InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<T>(), 0));
        }
    }
}