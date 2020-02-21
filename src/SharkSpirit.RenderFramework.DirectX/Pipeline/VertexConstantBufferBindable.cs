namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class VertexConstantBufferBindable<T> : ConstantBufferBindable<T> where T : struct
    {
        public VertexConstantBufferBindable(IDevice device) : base(device)
        {
        }

        public override void Bind()
        {
            Device.GetDeviceContext().VertexShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}