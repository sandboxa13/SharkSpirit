using SharkSpirit.RenderFramework.DirectX.Primitives.Sphere;
using SharpDX;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        public PixelConstantBufferStage(IDevice device) : base(device)
        {
        }

        public override void BindToPipeline()
        {
            var color = new SpherePrimitiveBuilder.ConstantColor(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));

            Update(color);

            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}