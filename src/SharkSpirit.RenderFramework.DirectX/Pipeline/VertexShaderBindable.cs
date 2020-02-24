using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.Pipeline
{
    public class VertexShaderBindable : BindableBase
    {
        private readonly VertexShader _vertexShader;

        
        public VertexShaderBindable(IDevice device, string path) : base(device)
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(path, "VS", "vs_4_0", ShaderFlags.Debug);

            _vertexShader = new VertexShader(device.GetDevice(), vertexShaderByteCode);
        }

        public override void Bind()
        {
            Device.GetDeviceContext().VertexShader.SetShader(_vertexShader, null, 0);
        }
    }
}