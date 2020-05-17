namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class VertexConstantBufferStage<T> : ConstantBufferStage<T> where T : unmanaged
    {
        public VertexConstantBufferStage(IDevice device) : base(device)
        {
        }
        public override void BindToPipeline()
        {
            Device.GetDeviceContext().VertexShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}