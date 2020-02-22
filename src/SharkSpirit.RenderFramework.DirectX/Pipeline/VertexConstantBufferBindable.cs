using SharkSpirit.Graphics;
using SharpDX.Direct3D11;

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

        public virtual void Update(ConstantBuffer consts, Buffer constantBuffer)
        {
            ConstantBuffer = constantBuffer;

            Device.GetDeviceContext().UpdateSubresource(ref consts, constantBuffer);
        }
    }
}