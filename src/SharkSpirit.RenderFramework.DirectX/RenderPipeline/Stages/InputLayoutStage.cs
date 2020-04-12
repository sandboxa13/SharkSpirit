using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class InputLayoutStage : StageBase
    {
        private readonly InputLayout _inputLayout;

        public InputLayoutStage(IDevice device, string path) : base(device)
        {
            var vertexShaderByteCode = ShaderBytecode.CompileFromFile(path, "VS", "vs_4_0", ShaderFlags.Debug);

            var signature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            _inputLayout = new InputLayout(device.GetDevice(), signature, new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0),
            });
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().InputAssembler.InputLayout = _inputLayout;
        }
    }
}