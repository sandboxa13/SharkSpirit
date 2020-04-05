namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelConstantBufferStage<T> : ConstantBufferStage<T> where T : struct
    {
        public PixelConstantBufferStage(IDevice device) : base(device)
        {
        }
        public override void BindToPipeLine()
        {
            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}