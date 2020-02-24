using SharpDX.Direct3D;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class TopologyBindable : BindableBase
    {
        private readonly PrimitiveTopology _primitiveTopology;

        public TopologyBindable(
            IDevice device, 
            PrimitiveTopology primitiveTopology) : base(device)
        {
            _primitiveTopology = primitiveTopology;
        }

        public override void Bind()
        {
            Device.GetDeviceContext().InputAssembler.PrimitiveTopology = _primitiveTopology;
        }
    }
}