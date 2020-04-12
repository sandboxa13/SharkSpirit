using SharpDX.Direct3D11;

namespace SharkSpirit.RenderFramework.DirectX.RenderPipeline.Stages
{
    public class InputLayoutStage : StageBase
    {
        private readonly InputLayout _inputLayout;

        public InputLayoutStage(IDevice device, InputLayout inputLayout) : base(device)
        {
            _inputLayout = inputLayout;
        }

        public override void BindToPipeline()
        {
            Device.GetDeviceContext().InputAssembler.InputLayout = _inputLayout;
        }
    }
}