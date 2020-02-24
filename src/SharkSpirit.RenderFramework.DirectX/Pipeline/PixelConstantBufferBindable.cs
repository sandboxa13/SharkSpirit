namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class PixelConstantBufferBindable<T> : ConstantBufferBindable<T> where T : struct
    {
        public PixelConstantBufferBindable(IDevice device) : base(device)
        {
        }

        public override void Bind()
        {
            Device.GetDeviceContext().PixelShader.SetConstantBuffer(0, ConstantBuffer);
        }
    }
}