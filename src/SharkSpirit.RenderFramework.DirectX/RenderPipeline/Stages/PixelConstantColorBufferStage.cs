using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantColorBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;

        public PixelConstantColorBufferStage(IDevice device, RenderObject renderObject, int slot = 0) : base(device, slot)
        {
            _renderObject = renderObject;
        }

        public override void BindToPipeline()
        {
            var cc = new SpherePrimitiveBuilder.ConstantColor
            {
                Color = _renderObject.Color
            };

            Update(cc);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(Slot, ConstantBuffer);
        }
    }
}
