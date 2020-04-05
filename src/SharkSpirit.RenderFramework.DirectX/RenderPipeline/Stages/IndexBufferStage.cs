using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class IndexBufferStage : StageBase 
    {
        private readonly Buffer _indexBuffer;
        private readonly int _count;

        public IndexBufferStage(
            IDevice device,
            ushort[] indices) : base(device)
        {
            var desc = IndexBufferDescriptionFactory.CreateIndexBufferDescription(indices.Length);

            _count = indices.Length;

            _indexBuffer = Buffer.Create(device.GetDevice(), indices, desc);
        }

        public int GetCount() => _count;

        public override void BindToPipeLine()
        {
            Device.GetDeviceContext().InputAssembler.SetIndexBuffer(_indexBuffer, Format.R16_UInt, 0);
        }
    }
}