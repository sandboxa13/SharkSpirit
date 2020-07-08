using SharkSpirit.Graphics;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly ObjectCBuf _pcb;

        public PixelConstantBufferStage(IDevice device, int slot = 0) : base(device, slot)
        {
        }

        public PixelConstantBufferStage(IDevice device, ObjectCBuf pcb, int slot = 0) : base(device, slot)
        {
            _pcb = pcb;
        }

        public void BindCustom<T>(T consts) where T : struct
        {
            Update(consts);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }

        public override void BindToPipeline()
        {
            Update(_pcb);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }
    }
}