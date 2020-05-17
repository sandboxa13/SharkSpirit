using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        private readonly RenderObject _renderObject;

        public PixelConstantBufferStage(IDevice device, RenderObject renderObject) : base(device)
        {
            _renderObject = renderObject;
        }

        public override void BindToPipeline()
        {
            var color = new SpherePrimitiveBuilder.ConstantColor(_renderObject.Color);

            Update(color);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}