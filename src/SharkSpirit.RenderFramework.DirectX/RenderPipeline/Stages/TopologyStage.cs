using SharpDX.Direct3D;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class TopologyStage : StageBase
    {
        private readonly PrimitiveTopology _primitiveTopology;

        public TopologyStage(
            IDevice device, 
            PrimitiveTopology primitiveTopology) : base(device)
        {
            _primitiveTopology = primitiveTopology;
        }

        public override void BindToPipeLine()
        {
            Device.GetDeviceContext().InputAssembler.PrimitiveTopology = _primitiveTopology;
        }
    }
}