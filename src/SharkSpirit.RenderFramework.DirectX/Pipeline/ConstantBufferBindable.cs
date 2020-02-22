using SharkSpirit.RenderFramework.DirectX.Pipeline.Factories;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class ConstantBufferBindable<T> : BindableBase where T : struct 
    {
        public ConstantBufferBindable(IDevice device) : base(device)
        {
            var cbd = ConstantBufferDescriptionFactory.CreateConstantBufferDescription();

            ConstantBuffer = new Buffer(device.GetDevice(), cbd);
        }
        protected  Buffer ConstantBuffer { get; set; }
        
        public override void Bind()
        {
            
        }
    }
}