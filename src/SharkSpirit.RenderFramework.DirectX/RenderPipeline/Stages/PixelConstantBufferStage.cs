using SharkSpirit.Graphics;
using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;
        private readonly ObjectCBuf _pcb;

        public PixelConstantBufferStage(IDevice device, RenderObject renderObject, int slot = 0) : base(device, slot)
        {
            _renderObject = renderObject;
        }

        public PixelConstantBufferStage(IDevice device, RenderObject renderObject, ObjectCBuf pcb, int slot = 0) : base(device, slot)
        {
            _renderObject = renderObject;
            _pcb = pcb;
        }

        public void BindCustom<T>(T consts) where T : struct
        {
            Update(consts);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }

        public override void BindToPipeline()
        {
            // var cc = new SpherePrimitiveBuilder.ConstantColor
            // {
            //     Color = _renderObject.Color
            // };
            //
            // Update(cc);
            
            if (_pcb.MaterialColor.X != 0)
            {
                Update(_pcb);
            }

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }
    }
}