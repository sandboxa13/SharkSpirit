using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class ConstantBufferStage<T> : StageBase where T : struct 
    {
        public ConstantBufferStage(IDevice device) : base(device)
        {
            var cbd = ConstantBufferDescriptionFactory.CreateConstantBufferDescription();

            ConstantBuffer = new Buffer(device.GetDevice(), cbd);
        }
        protected Buffer ConstantBuffer { get; set; }
        
        public virtual void Update(ConstantBuffer consts, Buffer constantBuffer)
        {
            ConstantBuffer = constantBuffer;

            Device.GetDeviceContext().UpdateSubresource(ref consts, constantBuffer);
        }

        public override void BindToPipeline()
        {
        }
    }
}