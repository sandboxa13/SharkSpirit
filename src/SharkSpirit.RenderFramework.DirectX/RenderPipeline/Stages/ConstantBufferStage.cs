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
        protected Buffer ConstantBuffer { get; set; }

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        public virtual void Update<T>(T consts, Buffer constantBuffer) where T :  struct
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            ConstantBuffer = constantBuffer;

            Device.GetDeviceContext().UpdateSubresource(ref consts, constantBuffer);
        }

        public override void BindToPipeline()
        {
        }
    }
}