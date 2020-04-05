using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class PixelShaderStage : StageBase
    {
        private readonly PixelShader _pixelShader;

        public PixelShaderStage(IDevice device, string path) : base(device)
        {
            var pixelShaderByteCode = ShaderBytecode.CompileFromFile(path, "PS", "ps_4_0", ShaderFlags.Debug);

            _pixelShader = new PixelShader(device.GetDevice(), pixelShaderByteCode);
        }

        public override void BindToPipeLine()
        {
            Device.GetDeviceContext().PixelShader.SetShader(_pixelShader, null, 0);
        }
    }
}