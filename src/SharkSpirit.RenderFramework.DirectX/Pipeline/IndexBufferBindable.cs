using SharkSpirit.RenderFramework.DirectX.Pipeline.Factories;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class IndexBufferBindable : BindableBase
    {
        private readonly Buffer _indexBuffer;
        private readonly int _count;
        
        public IndexBufferBindable(
            IDevice device,
            ushort[] indices) : base(device)
        {
            var desc = IndexBufferDescriptionFactory.CreateIndexBufferDescription(indices.Length);

            _count = indices.Length;

            _indexBuffer = Buffer.Create(device.GetDevice(), indices, desc);
        }

        protected override void Bind()
        {
            Device.GetDeviceContext().InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);
        }
    }
}