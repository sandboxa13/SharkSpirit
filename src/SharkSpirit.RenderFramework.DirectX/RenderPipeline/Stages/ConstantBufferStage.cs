using SharkSpirit.RenderFramework.DirectX.RenderPipeline.Factories;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class ConstantBufferStage<T> : StageBase where T : unmanaged 
    {
        public ConstantBufferStage(IDevice device, int slot) : base(device)
        {
            Slot = slot;
            var cbd = ConstantBufferDescriptionFactory.CreateConstantBufferDescription<T>();

            ConstantBuffer = new Buffer(device.GetDevice(), cbd);
        }
        public Buffer ConstantBuffer { get; set; }
        public int Slot { get; }

        public virtual void Update<T>(T consts) where T :  struct
        {
            Device.GetDeviceContext().MapSubresource(ConstantBuffer, MapMode.WriteDiscard, MapFlags.None, out var mappedResource);

            mappedResource.Write(consts);

            Device.GetDeviceContext().UnmapSubresource(ConstantBuffer, 0);
        }

        public override void BindToPipeline()
        {
        }
    }
}