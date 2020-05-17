using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class ConstantBufferStage<T> : StageBase where T : unmanaged 
    {
        public ConstantBufferStage(IDevice device) : base(device)
        {
            var cbd = ConstantBufferDescriptionFactory.CreateConstantBufferDescription<T>();

            ConstantBuffer = new Buffer(device.GetDevice(), cbd);
        }
        public Buffer ConstantBuffer { get; set; }
        
        public virtual void Update<T>(T consts) where T :  struct 
        {
            Device.GetDeviceContext().UpdateSubresource(ref consts, ConstantBuffer);
        }

        public override void BindToPipeline()
        {
        }
    }
}