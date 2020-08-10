using SharkSpirit.Graphics;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;
        private ObjectCBuf _pcb;

        public PixelConstantBufferStage(IDevice device, int slot = 0) : base(device, slot)
        {
        }

        public PixelConstantBufferStage(IDevice device, ObjectCBuf pcb,  RenderObject renderObject, int slot = 0) : base(device, slot)
        {
            _pcb = pcb;
            _renderObject = renderObject;
        }

        public void BindCustom<T>(T consts) where T : struct
        {
            Update(consts);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }

        public override void BindToPipeline()
        {
            _pcb.MaterialColor = new Vector3(_renderObject.Color.X, _renderObject.Color.Y, _renderObject.Color.Z);
            _pcb.SpecularIntensity = _renderObject.SpecularIntensity;
            _pcb.SpecularPower = _renderObject.SpecularPower;

            Update(_pcb);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }
    }
}