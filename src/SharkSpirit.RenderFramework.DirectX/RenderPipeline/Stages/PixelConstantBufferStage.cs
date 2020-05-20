using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;

        public PixelConstantBufferStage(IDevice device, RenderObject renderObject) : base(device)
        {
            _renderObject = renderObject;
        }

        public void BindCustom<T>(T consts) where T : struct
        {
            Update(consts);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }

        public override void BindToPipeline()
        {
            var cc = new SpherePrimitiveBuilder.ConstantColor
            {
                Color = _renderObject.Color
            };

            Update(cc);
            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}