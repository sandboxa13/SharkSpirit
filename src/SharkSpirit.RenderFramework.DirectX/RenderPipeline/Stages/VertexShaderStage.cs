using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class VertexShaderStage : StageBase
    {
        private readonly VertexShader _vertexShader;
        
        public VertexShaderStage(IDevice device, string path) : base(device)
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(path, "VS", "vs_4_0", ShaderFlags.Debug);

            _vertexShader = new VertexShader(device.GetDevice(), vertexShaderByteCode);
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().VertexShader.Set(_vertexShader);
        }
    }
}